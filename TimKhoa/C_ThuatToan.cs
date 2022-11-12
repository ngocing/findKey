using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimKhoa
{
    struct S_PhuToiThieu
    {
        public List<string> trai;

        public List<string> phai;
    }
    struct S_DichChuyen
    {
        public S_PhuToiThieu G;
        public string V;
    }

    class C_ThuatToan
    {
        /// <summary>
        /// Tìm bao đóng
        /// </summary>
        /// <param name="baoDong">Bao đóng cần tìm</param>
        /// <param name="trai">tập phục thuộc hàm bên trái</param>
        /// <param name="phai">tập phục thuộc hàm bên phải</param>
        /// <param name="n">số phục thuộc hàm</param>
        /// <returns>Bao đóng</returns>
        public string TimBaoDong(string baoDong, List<string> trai, List<string> phai)
        {
            int doDaiBaoDong = baoDong.Length-1;

            while (doDaiBaoDong != baoDong.Length)
            {
                doDaiBaoDong = baoDong.Length;

                for (int i = 0; i < trai.Count; i++)
                {
                    if (SoSanhChuoi(trai[i], baoDong))
                    {
                        for (int j = 0; j < phai[i].Length;j++ )
                            if (!SoSanhChuoi(phai[i][j].ToString(), baoDong))
                                baoDong += phai[i][j].ToString();
                    }
                }
    
            }

            return baoDong;
        }

        /// <summary>
        /// So sánh chuỗi A có nằm trong chuỗi B không
        /// </summary>
        /// <param name="con">A</param>
        /// <param name="cha">B</param>
        /// <returns>true nếu nằm trong, ngược lại trả về false</returns>
        private bool SoSanhChuoi(string con, string cha)
        {
            int ChuoiCon=0;

            if (cha.Length < con.Length)
                return false;

            for (int i = 0; i < con.Length; i++)
                for (int j = 0; j < cha.Length; j++)
                {
                    if (con[i] == cha[j])
                    {
                        ChuoiCon++;
                        break;
                    }
                }

                    if (ChuoiCon == con.Length)
                        return true;

            return false;
        }

        /// <summary>
        /// tìm tất cả các khóa của lược đồ
        /// </summary>
        /// <param name="tapThuocTinh">tập thuộc tính của lược đồ</param>
        /// <param name="trai">danh sách phụ thuộc hàm bên trái</param>
        /// <param name="phai">danh sách phụ thuộc hàm bên phải</param>
        /// <returns>trả về danh sách các khóa</returns>
        public List<string> TimKhoa(string tapThuocTinh, List<string> trai, List<string> phai)
        {
            List<string> listKhoa = new List<string>();

            string L="";
            string R="";
            string TN="";
            string TG="";

            //lấy tập L (chỉ xuất hiện vế trái, ko xh vế phải)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]))
                    {
                        L+=tapThuocTinh[i].ToString();
                        break;
                    }
            }
            //lấy tập R (chỉ xuất hiện vế phải, ko xh vế trái)
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                for (int t = 0; t < trai.Count; t++)
                    if (SoSanhChuoi(tapThuocTinh[i].ToString(), phai[t]) && !SoSanhChuoi(tapThuocTinh[i].ToString(), trai[t]))
                    {
                        R+=tapThuocTinh[i].ToString();
                        break;
                    }
            }

            /*lấy TN thuộc tính chỉ xuất hiện ở vế trái, không xuất hiện ở vế phải và
             * các thuộc tính không xuất hiện ở cả vế trái và vế phải của F*/
            for (int i = 0; i < tapThuocTinh.Length; i++)
            {
                if (!SoSanhChuoi(tapThuocTinh[i].ToString(), R))
                {
                    TN += tapThuocTinh[i].ToString();
                }
            }
            //lấy TG (giao giữa 2 tập L và R)
            for (int i = 0; i < L.Length; i++)
            {
                if (SoSanhChuoi(L[i].ToString(), R))
                {
                    TG += L[i].ToString();
                }
            }

            //nếu tập TG rỗng thì khóa chính là TN
            if (TG == "")
            {
                listKhoa.Add(TN);
                return listKhoa;
            }
            else
            {
                List<string> TapConTG = new List<string>();
                //sinh tập con của TG
                TapConTG = TimTapCon(TG);

                List<string> SieuKhoa = new List<string>();

                //kiểm tra từng tập con của TG hợp với TN có là siêu khóa không
                for (int n = 0; n < TapConTG.Count; n++)
                {
                    //lấy giao tập nguồn(TN) và từng con của TG 
                    string temp = TN + TapConTG[n];
                    //nếu giao tập nguồn(TN) và từng con của TG tất cả lấy bao đóng mà bằng tập thuộc tính thì là siêu khóa
                    if (SoSanhChuoi(tapThuocTinh, TimBaoDong(temp, trai, phai)))
                    {
                        SieuKhoa.Add(temp);
                    }
                }

                //tìm siêu khóa tối thiểu
                for (int i = 0; i < SieuKhoa.Count; i++)
                {
                    for (int j = i + 1; j < SieuKhoa.Count; j++)
                    {
                        if (SoSanhChuoi(SieuKhoa[i], SieuKhoa[j]))
                        {
                            SieuKhoa.Remove(SieuKhoa[j]);
                            j--;
                        }
                    }
                }

                listKhoa = SieuKhoa;
            }

            return listKhoa;
        }

        /// <summary>
        /// tìm con con bằng phương pháp sinh
        /// </summary>
        /// <param name="str">chuỗi cần sinh tập con</param>
        /// <returns>trả về danh sách tập con</returns>
        List<string> TimTapCon(string str)
        {

            List<string> TapCon = new List<string>();

            int[] a = new int[str.Length];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 0;
            }

            int t = str.Length - 1;

            TapCon.Add("");
            while (t >= 0)
            {

                t = str.Length - 1;
                while (t >= 0 && a[t] == 1)
                    t--;

                if (t >= 0)
                {
                    a[t] = 1;
                    for (int i = t + 1; i < str.Length; i++)
                        a[i] = 0;

                    string temp = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (a[i] == 1)
                        {
                            temp += str[i];
                        }
                    }

                    TapCon.Add(temp);
                }
            }

            return TapCon;
        }

        public S_PhuToiThieu TimPhuToiThieu(List<string> trai, List<string> phai)
        {
            S_PhuToiThieu ptt = new S_PhuToiThieu();

            int n = phai.Count;
            //tách phụ thuộc hàm vế phải có hơn 1 thuộc tính
            for (int i = 0; i < n; i++)
            {
                if (phai[i].Length > 1)
                {
                    string tempPhai = phai[i];
                    string temTrai = trai[i];

                    trai.Remove(trai[i]);
                    phai.Remove(phai[i]);

                    for (int j = 0; j < tempPhai.Length; j++)
                    {
                        trai.Add(temTrai);
                        phai.Add(tempPhai[j].ToString());
                    }

                    i--;
                }
            }

            //loại bỏ thuộc tính dư thừa bên vế trái có hơn 1 thuộc tính
            for (int i = 0; i < trai.Count; i++)
            {
                if (trai[i].Length > 1)
                {

                    for (int j = 0; j < trai[i].Length; j++)
                    {
                        if (trai[i].Length > 1)
                        {
                            string temp = trai[i];
                            temp = CatKiTu(temp, j);

                            if (SoSanhChuoi(phai[i], TimBaoDong(temp, trai, phai)))
                            {
                                trai[i] = temp;
                                j--;
                            }

                        }
                    }
                }
            }

            //loại bỏ thuộc tính dư thừa
            List<string> TempTrai = new List<string>();
            List<string> TempPhai = new List<string>();

            for (int i = 0; i < trai.Count; i++)
            {
                TempTrai.Add(trai[i]);
                TempPhai.Add(phai[i]);
            }

            for (int i = 0; i < trai.Count; i++)
            {

                TempTrai.RemoveAt(i);
                TempPhai.RemoveAt(i);

                if (SoSanhChuoi(phai[i], TimBaoDong(trai[i], TempTrai, TempPhai)))
                {
                    trai.Clear();
                    phai.Clear();

                    for (int t = 0; t < TempTrai.Count; t++)
                    {
                        trai.Add(TempTrai[t]);
                        phai.Add(TempPhai[t]);
                    }

                    i--;
                }
                else
                {
                    TempTrai.Clear();
                    TempPhai.Clear();

                    for (int t = 0; t < trai.Count; t++)
                    {
                        TempTrai.Add(trai[t]);
                        TempPhai.Add(phai[t]);
                    }
                }
            }

            ptt.trai = trai;
            ptt.phai = phai;

            return ptt;
        }

        public S_DichChuyen DichChuyenLDQH(List<string> trai, List<string> phai, String U, String M)
        {
            S_DichChuyen gv = new S_DichChuyen();
          //  S_PhuToiThieu g = new S_PhuToiThieu();
            List<string> gtrai = new List<string>();
            List<string> gphai = new List<string>();
            char[] arrayU = U.ToCharArray();
            char[] arrayM = M.ToCharArray();
            string v="";
            foreach(char u in arrayU)
            {
                if(!arrayM.Contains(u))
                {
                   v+= u;
                }
            }
            for (int i = 0; i < trai.Count; i++)
            {
                char[] arrayTrai = trai[i].ToCharArray();
                char[] arrayPhai = phai[i].ToCharArray();
                string strai="";
                string sphai = "";
                
                foreach (char item in arrayTrai)
                {
                    if(!arrayM.Contains(item))
                    {
                        strai+=item;
                    }
                }
                foreach (char item in arrayPhai)
                {
                    if (!arrayM.Contains(item))
                    {
                        sphai += item;
                    }
                }
                gtrai.Add(strai);
                gphai.Add(sphai);

            }
            gv.G = naturalReduced(gtrai, gphai);
            gv.V = v;
            return gv;
        }

        public S_PhuToiThieu naturalReduced(List<string> trai,List<string> phai)
        {
            S_PhuToiThieu G = new S_PhuToiThieu();
             List<string> gtrai = new List<string>();
             List<string> gphai = new List<string>();
  
            for(int i=0;i<trai.Count;i++)
            {
                string z = phai[i];
                foreach (char l in trai[i].ToCharArray())
                {
                    if(z.Contains(l))
                    {
                       z= z.Remove(z.IndexOf(l),1);
                    }
                }
                if (z!= "")
                {
                    int check= 0;
                    for (int j= 0; j<gtrai.Count;j++)
                    {
                        if(gtrai[j]== trai[i])
                        {
                            foreach (char l in gphai[j].ToCharArray())
                            {
                                if (!gphai[j].Contains(l))
                                {
                                    gphai[j] += l;
                                }
                            }
                            check = 1;
                        }
                    }
                    if(check==0)
                    {
                        gtrai.Add(trai[i]);
                        gphai.Add(z);
                    }
                }
            }
            G.trai = gtrai;
            G.phai = gphai;
            
            return G;
        }
        public string tinhBaoDong(List<string> trai, List<string> phai, string x)
        {
            string y = x,z="";
            do
            {
                 z = y;
                for(int i=0;i<trai.Count;i++)
                {
                    int dem = 0;
                    foreach(char ctrai in trai[i].ToCharArray())
                    {
                        if(!y.Contains(ctrai))
                        {
                            dem = 1;
                        }
                    }
                    if(dem==0)
                    {
                        y += phai[i];
                    }
                }
            } while (z != y);
            return y;
        }
        public string timBaoDong(List<string> trai, List<string> phai, string u)
        {
            string rs= "",ls="", u1=u, p="", x="";

            for (int i = 0; i < trai.Count; i++)
            {
                foreach(char ctrai in trai[i].ToCharArray())
                {
                    if(!ls.Contains(ctrai))
                    {
                        ls += ctrai;
                    }
                }
                foreach (char cphai in phai[i].ToCharArray())
                {
                    if (!rs.Contains(cphai))
                    {
                        rs += cphai;
                    }
                }
            }
            foreach(char crs in rs.ToCharArray())
            {
                if(u1.Contains(crs))
                {
                    u1 = u1.Remove(u1.IndexOf(crs),1);  
                }
            }
            p = rs;
            foreach (char cls in ls.ToCharArray())
            {
                if (p.Contains(cls))
                {
                    p = p.Remove(p.IndexOf(cls), 1);
                }
            }
            x = u1;
            foreach (char cp in p.ToCharArray())
            {
                if (!x.Contains(cp))
                {
                    x += cp;
                }
            }
            return x;
        }
        public string timGiaoCacKhoa(List<string> trai, List<string> phai,string u)
        {
            string m = u;
            for(int i=0;i<trai.Count;i++)
            {
                string phaix = phai[i];
                foreach(char ctrai in trai[i].ToCharArray())
                {
                    if(phai[i].Contains(ctrai))
                    {
                        phaix = phaix.Remove(phaix.IndexOf(ctrai),1);
                    }
                }
                foreach(char cphaix in phaix)
                {
                    if(m.Contains(cphaix))
                    {
                        m = m.Remove(m.IndexOf(cphaix),1);
                    }
                }
            }
            return m;
        }
        string CatKiTu(string str, int vitri)
        {
            string ok="";
            for (int i = 0; i < str.Length; i++)
            {
                if(vitri != i)
                    ok += str[i].ToString();
            }

            return ok;
        }
    }
}
