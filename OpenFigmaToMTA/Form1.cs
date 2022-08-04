using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenFigmaToMTA
{
    public partial class Form1 : Form
    {
        private Structs.Root _figmaData;

        public Form1()
        {
            InitializeComponent();

            if (System.IO.File.Exists("Key.txt"))
                txtKey.Text = System.IO.File.ReadAllText("Key.txt");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var key = txtKey.Text;
            var url = txtURL.Text;
            var bg = txtBG.Text;

            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Please fill the key to continue!");
                return;
            }

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please fill the url with your figma project to continue!");
                return;
            }

            if (string.IsNullOrEmpty(bg))
            {
                MessageBox.Show("Please select your background element to generate all childrens!");
                return;
            }

            if(!System.IO.File.Exists("Key.txt"))
                System.IO.File.WriteAllText("Key.txt", key);
            
            Task.WaitAll(LoadFigmaData(url, key));

            if (_figmaData == null)
            {
                MessageBox.Show("Something went wrong! Fill the form correctly!");
                return;
            }

            //Generate file here
            LuaGenerator gen = new LuaGenerator(_figmaData, bg);
            gen.GenerateLua();
            string genFile = string.Format("c{0}.lua", _figmaData.name);

            if (System.IO.File.Exists(genFile))
                System.IO.File.Delete(genFile);

            System.IO.File.WriteAllLines(genFile, gen.getResult().ToArray());
            MessageBox.Show(string.Format("File {0} generated, check the folder!", genFile));
        }

        private async Task LoadFigmaData(string url, string key)
        {
            _figmaData = await Functions.getFigmaContent(url, key);
        }
    }
}
