using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace HypermediaInspector
{
    public partial class HypermediaViewer : UserControl
    {
        private HypermediaInspector _baseInspector = null;
        string _jsonContent;
        public HypermediaViewer()
        {
            InitializeComponent();
        }

        public HypermediaViewer(HypermediaInspector baseInspector) : this()
        {
            _baseInspector = baseInspector;
        }



        public void Clear()
        {
            //this.Enabled = true;
            this.JsonTreeView.Nodes.Clear();
        }



        internal void Lock()
        {
            this.JsonTreeView.Nodes.Clear();
            //this.Enabled = false;
        }

        public string JsonContent
        {
            get
            {
                return _jsonContent;
            }
            set
            {
                _jsonContent = value;
                if (_jsonContent != null)
                {
                    JObject obj = JObject.Parse(_jsonContent);
                    JsonTreeView.Nodes.Clear();
                    TreeNode parent = Json2Tree(obj);
                    parent.Text = "JSON";
                    JsonTreeView.Nodes.Add(parent);
                    JsonTreeView.ExpandAll();
                    JsonTreeView.Nodes[0].EnsureVisible();
                }
                else
                    JsonTreeView.Nodes.Clear();
            }
        }


        // Code taken from 
        // http://stackoverflow.com/questions/18769634/creating-tree-view-dynamically-according-to-json-text-in-winforms
        private TreeNode Json2Tree(JObject obj)
        {
            //create the parent node
            TreeNode parent = new TreeNode();
            //loop through the obj. all token should be pair<key, value>
            foreach (var token in obj)
            {
                //change the display Content of the parent
                parent.Text = token.Key.ToString();
                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {
                    // child.Text = token.Key.ToString();
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o);
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        // Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                        }
                        // Array of arrays
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                CreateNode(dataArray, data, true);                                
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            // Array of simple types
                            CreateNode(child, itm, true);
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    //if token.Value is not nested
                    // child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                    {
                        CreateNode(child, token.Value, false);
                    }
                    parent.Nodes.Add(child);
                }
            }
            return parent;

        }

        private static TreeNode CreateNode(TreeNode child, JToken item, bool createNode)
        {
            string value = item.ToString();

            if (createNode)
            {
                child = child.Nodes.Add(value);
            }
            else
            {
                child.Text = $"{child.Text}={value}";
            }

            if (item.Type.ToString() == "String" &&
                Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                child.Tag = value;
                child.ForeColor = Color.Blue;
            }
            return child;
        }


        private void JsonTreeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = JsonTreeView.SelectedNode;

            if (node.ForeColor == Color.Blue)
            {
                _baseInspector.CallApi(node.Tag.ToString());
            }
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Text != e.Node.Tag.ToString())
            {
                string[] parts = e.Node.Text.Split(new char[] { '=' }, 2);
                if (parts.Length > 1)
                {
                    string text = parts[0] + "=";
                    Font normalFont = e.Node.TreeView.Font;

                    float textWidth = e.Graphics.MeasureString(text, normalFont).Width;
                    e.Graphics.DrawString(text,
                                          normalFont,
                                          SystemBrushes.WindowText,
                                          e.Bounds);

                    SolidBrush blueBrush = new SolidBrush(Color.Blue);

                    e.Graphics.DrawString(parts[1],
                                          normalFont,
                                          blueBrush,
                                          e.Bounds.Left + textWidth,
                                          e.Bounds.Top);
                }
                else
                {
                    e.DrawDefault = true;
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }
    }
}
