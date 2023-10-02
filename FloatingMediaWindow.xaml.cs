using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dev_interface
{
    public partial class FloatingMediaWindow : Window
    {
        // Propriedade para vincular o MediaElement existente
        public MediaElement MediaElementToDisplay { get; set; }

        // Propriedade para armazenar a referência ao elemento pai original
        public Panel OriginalParent { get; set; }

        public FloatingMediaWindow()
        {
            InitializeComponent();
            DataContext = this;
            // Manipuladores de eventos de mouse para permitir o movimento da janela
            MouseLeftButtonDown += FloatingMediaWindow_MouseLeftButtonDown;
            MouseMove += FloatingMediaWindow_MouseMove;
        }       
        private void FloatingMediaWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void FloatingMediaWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Verifica se o botão esquerdo do mouse está pressionado
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void FloatingMediaWindow_Closed(object sender, EventArgs e, ContentControl contentControl)
        {
            // Verifica se o MediaElementToDisplay não é nulo e se a propriedade Tag é um Panel
            if (MediaElementToDisplay != null && Tag is Panel originalParent)
            {
                contentControl.Content = null;

                // Restaura o MediaElement ao pai original
                originalParent.Children.Add(MediaElementToDisplay);
            }
        }
    }
}
