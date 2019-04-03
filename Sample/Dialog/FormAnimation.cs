using AuExtension.Extend.FormAnimator;
using System;
//using namespace
using System.Windows.Forms;


namespace Sample.Dialog
{
    public partial class FormAnimation : Form
    {
        private FormAnimator _animator;

        public FormAnimation()
        {
            InitializeComponent();
            FormAnimator.AnimationMethod animation = FormAnimator.AnimationMethod.Slide;
            FormAnimator.AnimationDirection direction = FormAnimator.AnimationDirection.Right;
            _animator = new FormAnimator(this, animation, direction, 500);

            this.Shown += FormAnimation_Shown;
        }

        private void FormAnimation_Shown(object sender, EventArgs e)
        {
            // Close the form by sliding down.
            //_animator.Duration = 0;
            System.Threading.Thread.Sleep(3000);
            this.Hide();
        }
    }
}
