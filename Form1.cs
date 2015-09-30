
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace ExplorerExam
{
    public partial class Form1 : Form
    {
        private void threadProc()
        {
            int ic = 0;
            for ( ; ; )
            {
                if (ic != count)
                {
                    listBox1.Items.Clear();
                    foreach (string str in selectFolder)
                    {
                        listBox1.Items.Add(str);
                    }
                    ic = count;
                }
                label1.Text = "point Path = " + PointPath.ToString();
            }
        }
        private List<string> selectFolder;
        private string[] strRoot;
        private static int PointPath = 0;
        private static int count = 0;
        private bool poinNULL = true;
        Thread t;
        public Form1()
        {
            InitializeComponent();
            imageList2.Images.Add(SystemIcons.WinLogo);
            t = new Thread(new ThreadStart(threadProc));
            t.Start();
            selectFolder = new List<string>();
            selectFolder.Add("root");
            strRoot = Directory.GetLogicalDrives();
            int size = strRoot.Length;

            for (int i = 0; i < size; i++)
            {

                try
                {
                    treeView1.Nodes.Add(strRoot[i]);
                    treeView1.Nodes[treeView1.Nodes.Count - 1].ImageIndex = 2;
                    DirectoryInfo info = new DirectoryInfo(strRoot[i] + "/");
                    DirectoryInfo[] sd = info.GetDirectories();
                    foreach (DirectoryInfo inf in sd)
                    {

                        treeView1.Nodes[i].Nodes.Add(inf.Name);
                        treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].Tag = inf.FullName;
                        try
                        {
                            if (new DirectoryInfo(inf.FullName).GetDirectories()[0] != null)
                            {
                                treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].Nodes.Add("temp");
                                treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].Nodes[treeView1.Nodes[i].Nodes[treeView1.Nodes[i].Nodes.Count - 1].Nodes.Count - 1].Tag = "temp";
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                ViewRootDir();
            }
        }
        private void ViewRootDir()
        {
            listView1.Items.Clear();
            int size = strRoot.Length;
            for (int i = 0; i < size; i++)
            {
                try
                {
                    listView1.Items.Add(strRoot[i]);
                    listView1.Items[listView1.Items.Count - 1].ImageIndex = 1;
                    listView1.Items[listView1.Items.Count - 1].Tag = strRoot[i];

                }
                catch (Exception ex)
                {
                }
            }
            AddressString.Text = selectFolder[0];
        }
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Tag == "temp")
            {
                e.Node.Nodes[0].Remove();
                DirectoryInfo info = new DirectoryInfo(e.Node.Tag.ToString());
                DirectoryInfo[] sd = info.GetDirectories();

                foreach (DirectoryInfo inf in sd)
                {
                    e.Node.Nodes.Add(inf.Name);
                    e.Node.Nodes[e.Node.Nodes.Count - 1].Tag = inf.FullName;
                    try
                    {
                        if (new DirectoryInfo(inf.FullName).GetDirectories()[0] != null)
                        {
                            e.Node.Nodes[e.Node.Nodes.Count - 1].Nodes.Add("ppp");
                            e.Node.Nodes[e.Node.Nodes.Count - 1].Nodes[e.Node.Nodes[e.Node.Nodes.Count - 1].Nodes.Count - 1].Tag = "temp";
                        }
                    }
                    catch (Exception e1)
                    {

                    }
                }
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedImageIndex = treeView1.SelectedNode.ImageIndex;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView lv = sender as ListView;
                string str = lv.SelectedItems[0].Tag.ToString();
                ShowDir(str);
                if (PointPath == 1)
                {
                    selectFolder.Clear();
                    selectFolder.Add("root");
                    count = 1;
                    PointPath = 0;
                }
                selectFolder.Add(str);
                PointPath = selectFolder.Count;
                count++;
            }
            catch (Exception ex)
            {
                try
                {
                    ListView lv = sender as ListView;
                    string str = lv.SelectedItems[0].Text;
                    System.Diagnostics.Process.Start(selectFolder[PointPath-1] + str);
                }
                catch (Exception e1)
                { }
            }
        }

        private void ShowDir(string lv)
        {
            try
            { 
                DirectoryInfo info = new DirectoryInfo(lv);
                FileInfo[] fileInfo = info.GetFiles();
                DirectoryInfo[] inf = info.GetDirectories();
                listView1.Items.Clear();
                foreach (DirectoryInfo i in inf)
                {
                    listView1.Items.Add(i.Name);
                    listView1.Items[listView1.Items.Count - 1].Tag = i.FullName;
                    listView1.Items[listView1.Items.Count - 1].ImageIndex = 2;
                }
                foreach (FileInfo i in fileInfo)
                {
                    listView1.Items.Add(i.Name);
                    imageList2.Images.Add(Icon.ExtractAssociatedIcon(i.FullName));
                    listView1.Items[listView1.Items.Count - 1].ImageIndex = imageList2.Images.Count-1;
                }
                AddressString.Text = lv;
                
            }
            catch (Exception e2)
            {
                throw;
                //MessageBox.Show("Error");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                ShowDir(e.Node.FullPath);
                if (selectFolder[selectFolder.Count - 1] != e.Node.FullPath)
                {
                    selectFolder.Add(e.Node.FullPath);
                    PointPath = selectFolder.Count;
                    count++;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectFolder[PointPath - 2] != "root")
                {
                    ShowDir(selectFolder[PointPath - 2]);
                    PointPath--;
                }
                else
                {
                    ViewRootDir();
                    PointPath--;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (PointPath - 1 < count)
                {
                    ShowDir(selectFolder[PointPath]);
                    PointPath++;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AddressString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    ShowDir(AddressString.Text);
                    if (selectFolder[selectFolder.Count - 1] != AddressString.Text)
                    {
                        selectFolder.Add(AddressString.Text);
                        PointPath = selectFolder.Count;
                        count++;
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var listView = sender as ListView;

            // Вызываем метод DoDragDrop чтобы начать перетаскивание.
            // Первый аргумент - это то, что мы хотим перетащить. В данном случае это все выделенные элементы.
            // Можно воспользоваться свойством e.Item, но тогда мы сможем перетаскивать только один элемент за раз.
            // Второй агумент - это действие, которое мы позволяем делать с элементом. В данном случае - перенести на другой лист
            listView.DoDragDrop(listView.SelectedItems, DragDropEffects.Move);
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;

            // Суть этого метода - поставить необходимое значение e.Effect. То есть какое действие должно выполниться после отпускания кнопки мыши:
            //  копирование, перенесение и т.д.
            // Значение этого параметра должно попадать в список разрешенных эффектов, установленных в методе DoDragDrop

            // В данном случае мы проверяем чтобы элементы, которые перетаскиваются, принадлежали определенному типу (иначе можно перетаскивать все подряд: файлы, ссылки, другие элементы).
            // То есть проверяем чтобы перетаскиваемые элементы были именно коллекцией выделенных элементов из контрола.
            // Если проверка проходит и разрешенный эффект совпадает с тем, что мы хотим сделать, ставим значение e.Effect на нужное действие - то, которое выполнится при отпускании мыши.
            if (e.Data.GetDataPresent("System.Windows.Forms.ListView+SelectedListViewItemCollection") && e.AllowedEffect == DragDropEffects.Move)
                e.Effect = DragDropEffects.Move;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            // Здесь выполняется непосредственное добавление/удаление элементов.
            // Если обработчик события DragEnter не разрешил действие, то после отпускания мыши это событие не будет вызвано.
            var listView = sender as ListView;

            // Достаем перетаскиваемые данные и приводим их к нужному нам типу - коллекции выделенных элементов.
            var items = e.Data.GetData("System.Windows.Forms.ListView+SelectedListViewItemCollection") as ListView.SelectedListViewItemCollection;

            // Начинаем обработку каждого элемента в коллекции
            foreach (ListViewItem item in items)
            {
                item.ListView.Items.Remove(item);	// Так как мы перетаскиваем из одного листа в другой, то элемент сначала надо удалить из "родительского" листа
                listView.Items.Add(item);           // Добавляем элемент в лист, на который его сбросили
            }
        }
        
    }
}
