public class MyForm : System.Windows.Forms.Form
{
    public MyForm()
    {
		this.Text = "C# Program";
		var txtINPUT = new System.Windows.Forms.TextBox();
		txtINPUT.Name = "txtINPUT";
		this.Controls.Add(txtINPUT);
		txtINPUT.BringToFront();
		txtINPUT.Dock = System.Windows.Forms.DockStyle.Fill;
		txtINPUT.Multiline = true;
		txtINPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		txtINPUT.Text = Mx.t2.MyData();
		//System.Windows.Forms.MessageBox.Show("UrText");
		//this.Close();
    }

    public static void Main()
    {
        System.Windows.Forms.Application.Run(new MyForm());
    }
}
