﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimKhoa
{
    public partial class Menu : Form
    {
        C_ThuatToan tt;
        List<string> listTrai;
        List<string> listPhai;

        public Menu()
        {
            InitializeComponent();

            tt = new C_ThuatToan();
            listTrai = new List<string>();
            listPhai = new List<string>();
        }
        //thêm phục thuộc hàm vào listBox1
        private void btThem_Click(object sender, EventArgs e)
        {
            if (txtTrai.Text != "" && txtPhai.Text != "")
            {
                listTrai.Add(txtTrai.Text.ToUpper());
                listPhai.Add(txtPhai.Text.ToUpper());

                listBox1.Items.Add(txtTrai.Text.ToUpper() + " -> " + txtPhai.Text.ToUpper());
                txtTrai.Clear();
                txtPhai.Clear();

                txtTrai.Focus();
            }
            else
                MessageBox.Show("Hãy nhập phụ thuộc hàm vào!!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //tìm khóa
        private void btTimKhoa_Click(object sender, EventArgs e)
        {
            if (txtTapThuocTinh.Text != "" && listBox1.Items.Count > 0)
            {
                listBox2.Items.Clear();

                List<string> Khoa = new List<string>();
                Khoa = tt.TimKhoa(txtTapThuocTinh.Text, listTrai, listPhai);

                for (int i = 0; i < Khoa.Count; i++)
                {
                    listBox2.Items.Add(Khoa[i].ToUpper());
                }
            }
            else
                MessageBox.Show("Bạn chưa nhập phụ thuộc hàm hoặc tập thuộc tính của lược đồ!!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //tìm bao đóng
        private void btTimBaoDong_Click(object sender, EventArgs e)
        {
            if (txtBaoDong.Text != "" && listBox1.Items.Count > 0)
            {
                MessageBox.Show("Bao đóng là: " + tt.TimBaoDong(txtBaoDong.Text.ToUpper(), listTrai, listPhai).ToUpper(), "Kết quả", MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
               
            else
                MessageBox.Show("Hãy nhập bao đóng cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //tìm phủ tối thiểu
        private void btPhuToiThieu_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox2.Items.Clear();

                S_PhuToiThieu ptt = new S_PhuToiThieu();

                ptt = tt.TimPhuToiThieu(listTrai, listPhai);

                for (int i = 0; i < ptt.phai.Count; i++)
                    listBox2.Items.Add(ptt.trai[i].ToUpper() + " -> " + ptt.phai[i].ToUpper());
            }
            else
                MessageBox.Show("Hãy nhập phụ thuộc hàm vào!!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // xóa hết trong listBox1
        private void btXoaHet_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listTrai = new List<string>();
            listPhai = new List<string>();
        }
        //xóa từng phần tử được trọn trọng listBox1
        private void btXoa_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }
        //nhập mới lại từ đầu
        private void btNhapLai_Click(object sender, EventArgs e)
        {
            listTrai = new List<string>();
            listPhai = new List<string>();

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            txtBaoDong.Clear();
            txtTrai.Clear();
            txtPhai.Clear();
            txtTapThuocTinh.Clear();

            txtTrai.Focus();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void phầnMềmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTin tt = new ThongTin();

            tt.Show();
        }

        private void btnDichchuyenLDQH_Click(object sender, EventArgs e)
        {
            Dichchuyen fdichchuyen = new Dichchuyen(listTrai,listPhai,txtTapThuocTinh.Text.ToUpper());
           
            fdichchuyen.Show();
        }

        private void btnRutgon_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox2.Items.Clear();
                List<string> trai = new List<string>();
                List<string> phai = new List<string>();
                trai = listTrai;
                phai = listPhai;

                S_PhuToiThieu ptt = new S_PhuToiThieu();

                ptt = tt.naturalReduced(trai, phai);

                for (int i = 0; i < ptt.phai.Count; i++)
                    listBox2.Items.Add(ptt.trai[i].ToUpper() + " -> " + ptt.phai[i].ToUpper());
            }
            else
                MessageBox.Show("Hãy nhập phụ thuộc hàm vào!!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void txtTapThuocTinh_Validated(object sender, EventArgs e)
        {
            txtTapThuocTinh.Text = txtTapThuocTinh.Text.ToUpper();
        }

        private void bt_3nf_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox2.Items.Clear();

                S_PhuToiThieu ptt = new S_PhuToiThieu();

                ptt = tt.TimPhuToiThieu(listTrai, listPhai);
                List<string> trai = new List<string>();
                List<string> phai = new List<string>();
                trai = listTrai;
                phai = listPhai;
                ptt = tt.naturalReduced(trai, phai);

                for (int i = 0; i < ptt.phai.Count; i++)
                {
                    
                    if (trai[i] == phai[i + 1] && trai[i + 1] == phai[i])
                    {
                        //ptt.trai[i] = trai[i] + trai[i + 1];
                        ptt.phai[i] = phai[i + 1];
                        listBox2.Items.Add("R" + i + "(" + ptt.trai[i].ToUpper() + ptt.phai[i].ToUpper() + ")");
                    }
                    else if (trai[i] == trai[i + 1]) 
                    {
                        ptt.phai[i] = phai[i] + phai[i + 1];
                        listBox2.Items.Add("R" + i + "(" + ptt.trai[i].ToUpper() + ptt.phai[i].ToUpper() + ")");
                        
                    }
                    else
                    {
                        listBox2.Items.Add("R" + i + "(" + ptt.trai[i].ToUpper() + ptt.phai[i].ToUpper() + ")");
                    }
                }
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
