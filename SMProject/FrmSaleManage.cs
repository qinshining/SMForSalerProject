using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;


namespace SMProject
{
    public partial class FrmSaleManage : Form
    {
        private LoginLogService objLogService = new LoginLogService();
        private ProductService objProductService = new ProductService();
        private SalesList mainSaleList = new SalesList();
        private BindingSource bs = new BindingSource();

        #region  窗体拖动、关闭【实际项目中不用】

        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        #endregion

        public FrmSaleManage()
        {
            InitializeComponent();
            //初始化收款员
            this.lblSalePerson.Text = Program.currentSalesPerson.SPName;
            this.KeyPreview = true;//允许窗体捕获键盘输入
            this.dgvProdutList.AutoGenerateColumns = false;//禁止自动生成列
        }

        /// <summary>
        /// 创建流水号（可能会出现异常），调用时需处理
        /// </summary>
        /// <returns></returns>
        private string CreateSerialNum()
        {
            string serialNum = DALCommon.GetServerTime().ToString("yyyyMMddHHmmssfff");
            Random random = new Random();
            serialNum += random.Next(10, 100).ToString();
            return serialNum;
        }

        #region 主窗体接收keydown事件

        private void FrmSaleManage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 112)//F1 进入结算
            {
                if (this.dgvProdutList.RowCount == 0)
                {
                    return;
                }
                ApplyBalance();
            }
            else if (e.KeyValue == 121)//F10 退出系统
            {
                this.Close();
            }
            else if (e.KeyValue == 38)//向上键
            {
                this.bs.MovePrevious();
            }
            else if (e.KeyValue == 40)//向下键
            {
                this.bs.MoveNext();
            }
            else if (e.KeyValue == 46)//delete键
            {
                if (this.dgvProdutList.CurrentRow == null)
                {
                    return;
                }
                this.bs.RemoveCurrent();
                //更新序号
                for (int i = 0; i < this.mainSaleList.SalesListDetail.Count; i++)
                {
                    this.mainSaleList.SalesListDetail[i].Num = i + 1;
                }
                this.lblTotalMoney.Text = (from p in this.mainSaleList.SalesListDetail select p.SubTotalMoney).Sum().ToString();
                if (this.bs.Count == 0)//清空之后需要重新new，不然会引发dgv异常
                {
                    this.bs = new BindingSource();
                }
                this.dgvProdutList.DataSource = null;
                this.dgvProdutList.DataSource = this.bs;
            }
        }
        #endregion

        #region 商品结算
        private void ApplyBalance()
        {
            FrmBalance frmBalance = new FrmBalance(this.lblTotalMoney.Text);
            DialogResult result = frmBalance.ShowDialog();
            if (result == DialogResult.OK)
            {
                string returnInfo = frmBalance.Tag.ToString();
                Member objMember = null;
                //输入了正确的会员号
                if (returnInfo.Contains("|"))
                {
                    string[] infoSplit = returnInfo.Split('|');
                    this.lblReceivedMoney.Text = infoSplit[0];
                    objMember = new Member()
                    {
                        MemberId = Convert.ToInt32(infoSplit[1]),
                        Points = (int)(Convert.ToDecimal(this.lblTotalMoney.Text) / 10)
                    };
                }
                else
                {
                    this.lblReceivedMoney.Text = returnInfo;
                }
                this.lblReturnMoney.Text = (Convert.ToDecimal(this.lblReceivedMoney.Text) - Convert.ToDecimal(this.lblTotalMoney.Text)).ToString();
                //封装销售主表
                SalesList objSalesList = new SalesList()
                {
                    SeriaNum = this.lblSerialNum.Text,
                    TotalMoney = Convert.ToDecimal(this.lblTotalMoney.Text),
                    RealReceive = Convert.ToDecimal(this.lblReceivedMoney.Text),
                    ReturnMoney = Convert.ToDecimal(this.lblReturnMoney.Text),
                    SalesPersonId = Program.currentSalesPerson.SalesPersonId
                };
                //封装销售明细
                foreach (SalesListDetail item in this.mainSaleList.SalesListDetail)
                {
                    objSalesList.SalesListDetail.Add(new SalesListDetail()
                    {
                        SerialNum = objSalesList.SeriaNum,
                        ProductId = item.ProductId,
                        ProductFullName = item.ProductFullName,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        Quantity = item.Quantity,
                        SubTotalMoney = item.SubTotalMoney
                    });
                }
                try
                {
                    //调用后台方法保存数据
                    objProductService.SaveSaleInfo(objSalesList, objMember);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("结算过程出现异常，请检查：" + ex.Message);
                    return;
                }
                //打印小票（电脑蓝屏。。）
                //this.mainSaleList = objSalesList;
                //this.printDocument.Print();
                //界面刷新
                ResetForm();
            }
            else
            {
                if (frmBalance.Tag.ToString().Equals("F4"))//修改商品列表
                {
                    this.lblReceivedMoney.Text = "0.00";
                    this.lblReturnMoney.Text = "0.00";
                    this.txtProductId.Focus();
                }
                else if (frmBalance.Tag.ToString().Equals("F5"))
                {
                    this.lblTotalMoney.Text = "0.00";
                    this.lblReceivedMoney.Text = "0.00";
                    this.lblReturnMoney.Text = "0.00";
                    ResetForm();
                }
            }
        }

        #endregion

        #region 结算后或取消交易刷新界面
        private void ResetForm()
        {
            this.mainSaleList = new SalesList();
            this.dgvProdutList.DataSource = null;
            this.lblSerialNum.Text = string.Empty;
            this.txtProductId.Focus();
        }
        #endregion

        #region 扫码添加商品

        private void TxtProductId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtProductId.Text.Trim().Length != 0 && e.KeyValue == 13)//回车键
            {
                if (!DataValidate.IsInteger(this.txtQuantity.Text.Trim()))//查找商品前先检查数量输入是否正确
                {
                    MessageBox.Show("数量输入不正确", "校验提示");
                    this.txtQuantity.SelectAll();
                    this.txtQuantity.Focus();
                    return;
                }
                try
                {
                    if (this.dgvProdutList.RowCount == 0 || this.lblSerialNum.Text.Length == 0)//第一次扫描生成序列号
                    {
                        this.lblTotalMoney.Text = "0.00";
                        this.lblReceivedMoney.Text = "0.00";
                        this.lblReturnMoney.Text = "0.00";
                        this.lblSerialNum.Text = CreateSerialNum();
                    }
                    var pList = from p in this.mainSaleList.SalesListDetail where p.ProductId.Equals(this.txtProductId.Text.Trim()) select p;
                    if (pList.Count() > 0)//已经在列表中
                    {
                        SalesListDetail objSalesDetail = pList.FirstOrDefault<SalesListDetail>();
                        objSalesDetail.Quantity += Convert.ToInt32(this.txtQuantity.Text.Trim());
                        objSalesDetail.SubTotalMoney = objSalesDetail.Quantity * objSalesDetail.UnitPrice;
                        if (objSalesDetail.Discount != 0)
                        {
                            objSalesDetail.SubTotalMoney *= Convert.ToDecimal(objSalesDetail.Discount / 10);
                            objSalesDetail.SubTotalMoney = Decimal.Round(objSalesDetail.SubTotalMoney, 2);
                        }
                    }
                    else
                    {
                        bool addResult = AddProductList();
                        if (!addResult)
                        {
                            return;
                        }
                    }
                    //显示数据（测试不使用BindingSource）
                    this.dgvProdutList.DataSource = null;
                    this.bs.DataSource = this.mainSaleList.SalesListDetail;
                    //if (this.mainSaleList.SalesListDetail != null && this.mainSaleList.SalesListDetail.Count > 0)//第一次绑定时候判断是否有值，否则会引发点击dgv异常
                    //this.dgvProdutList.DataSource = this.mainSaleList.SalesListDetail;
                    this.dgvProdutList.DataSource = this.bs;
                    this.bs.MoveLast();
                    //更新商品总金额
                    this.lblTotalMoney.Text = (from d in this.mainSaleList.SalesListDetail select d.SubTotalMoney).Sum().ToString();
                    //还原输入列表
                    this.txtProductId.Clear();
                    this.txtQuantity.Text = "1";
                    this.txtUnitPrice.Text = "0.00";
                    this.txtDiscount.Text = "0";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("扫码出现异常，请重试" + ex.Message, "错误提示");
                    return;
                }
            }
        }
        //添加新商品到销售明细，返回false则表明失败
        private bool AddProductList()
        {
            Product product = objProductService.GetProductInfo(this.txtProductId.Text.Trim());
            if (product == null)//未查到商品的情况下，允许手工添加
            {
                DialogResult result = MessageBox.Show("未查询到商品信息，手工录入商品，请检查单价和折扣是否录入正确，确定吗？", "询问信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return false;
                }
                //先判断单价和折扣是否输入正确
                if (!DataValidate.IsDecimalNum(this.txtUnitPrice.Text.Trim()) || Convert.ToDecimal(this.txtUnitPrice.Text.Trim()) == 0)
                {
                    MessageBox.Show("手工录入商品，单价输入不正确", "提示信息");
                    this.txtUnitPrice.SelectAll();
                    this.txtUnitPrice.Focus();
                    return false;
                }
                if (!DataValidate.IsDecimalNum(this.txtDiscount.Text.Trim()))
                {
                    MessageBox.Show("手工录入商品，折扣输入不正确", "提示信息");
                    this.txtDiscount.SelectAll();
                    this.txtDiscount.Focus();
                    return false;
                }
                product = new Product()
                {
                    ProductId = this.txtProductId.Text.Trim(),
                    ProductFullName = "手工录入商品",
                    UnitPrice = Convert.ToDecimal(this.txtUnitPrice.Text.Trim()),
                    Discount = Convert.ToDouble(this.txtDiscount.Text.Trim())
                };
            }
            SalesListDetail objSaleDetail = new SalesListDetail()
            {
                ProductId = product.ProductId,
                ProductFullName = product.ProductFullName,
                UnitPrice = product.UnitPrice,
                Discount = product.Discount,
                Quantity = Convert.ToInt32(this.txtQuantity.Text.Trim())
            };
            objSaleDetail.SubTotalMoney = objSaleDetail.Quantity * objSaleDetail.UnitPrice;
            if (objSaleDetail.Discount != 0)
            {
                objSaleDetail.SubTotalMoney *= Convert.ToDecimal(objSaleDetail.Discount / 10);
                objSaleDetail.SubTotalMoney = Decimal.Round(objSaleDetail.SubTotalMoney, 2);
            }
            objSaleDetail.Num = this.mainSaleList.SalesListDetail.Count + 1;
            this.mainSaleList.SalesListDetail.Add(objSaleDetail);
            return true;
        }

        #endregion

        #region 其他输入框回车事件

        private void TxtOther_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Length != 0 && e.KeyValue == 13)
                {
                    this.txtProductId.SelectAll();
                    this.txtProductId.Focus();
                }
            }
        }

        #endregion

        #region 退出系统，记录退出时间
        private void FrmSaleManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出吗？", "退出询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    DateTime exitTime = DALCommon.GetServerTime();
                    int count = objLogService.WriteExitLog(Program.currentSalesPerson.LogId, exitTime);
                    if (count == 1)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        MessageBox.Show("更新退出时间失败，请检查是否非法登录");
                        e.Cancel = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("退出时出现异常，请重试：" + ex.Message);
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region 打印事件
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Common.USBPrint.Print(e, this.mainSaleList.SalesListDetail, this.lblSerialNum.Text, this.lblSalePerson.Text);
        }
        #endregion
    }
}
