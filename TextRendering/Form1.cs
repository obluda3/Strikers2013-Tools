namespace TextRendering
{
    public partial class Form1 : Form
    {
        private TextRenderer _renderer = new TextRenderer();
        public Form1()
        {
            InitializeComponent();
        }
        private void update(int xadvance)
        {
            var bitmap = _renderer.RenderedText(textBox1.Text, xadvance, (int)numericUpDown2.Value);
            pictureBox1.Image = bitmap;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            update((int)numericUpDown1.Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            update((int)numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            update((int)numericUpDown1.Value);
        }
    }
}