namespace TextRendering
{
    public partial class Form1 : Form
    {
        private TextRenderer _renderer;
        public Form1()
        {
            InitializeComponent();
            GameBackground screen = new GameBackground(0, 0, 200, new Bitmap(320, 320));
            _renderer = new TextRenderer(screen);
        }
        private void update(int xadvance)
        {
            bool exceeds = false;
            var bitmap = _renderer.RenderedText(textBox1.Text, xadvance, (int)numericUpDown2.Value, out exceeds);
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