using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimKhoa
{
    public partial class Dichchuyen : Form
    {
        C_ThuatToan tt;
        List<string> listTrai;
        List<string> listPhai;
        public Dichchuyen(List<string> trai,List<string> phai,string u)
        {
            InitializeComponent();
            tt = new C_ThuatToan();
            S_PhuToiThieu g = new S_PhuToiThieu();
            g = tt.naturalReduced(trai, phai);
            listTrai = g.trai;
            listPhai = g.phai;
            string x = tt.timBaoDong(listTrai, listPhai,u);
            txbU.Text = u;
            txbM.Text = tt.TimBaoDong(x,g.trai,g.phai);
            string f = "";
            for (int i = 0; i < listTrai.Count; i++)
                f+= listTrai[i].ToUpper() + " -> " + listPhai[i].ToUpper()+", ";
            txbF.Text=f;
        }

        private void btnDichChuyen_Click(object sender, EventArgs e)
        {
            if (txbF.Text == "" || txbU.Text == "" || txbM.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ dữ liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> l, r = new List<string>();
            l = listTrai;
            r = listPhai;

            S_DichChuyen dichchuyen= tt.DichChuyenLDQH(l, r,txbU.Text.ToUpper(), txbM.Text.ToUpper());

            string g = "";
            for (int i = 0; i < dichchuyen.G.phai.Count; i++)
            {
                string trai = dichchuyen.G.trai[i].ToUpper();
                string phai = dichchuyen.G.phai[i].ToUpper();
                if (trai == "")
                    trai = "0";
                if (phai == "") continue;
                g += trai+" -> " + dichchuyen.G.phai[i].ToUpper() + " ,";
            }
                
            txbG.Text = g.ToUpper();
            txbV.Text = dichchuyen.V;
        }

        private void txbM_Validated(object sender, EventArgs e)
        {
            txbM.Text = txbM.Text.ToUpper();
        }

        private void Dichchuyen_Load(object sender, EventArgs e)
        {

        }
    }
}
