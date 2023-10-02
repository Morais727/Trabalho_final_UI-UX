using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TagLib;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dev_interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        private bool isMuted = false; 
        private bool isFullScreen = false;
        private Rect previousVideoPosition;
        private Panel originalParent; 

               
        public MainWindow()
        {
            InitializeComponent();
            LoadSongs();
            LoadVideos();
            // Registra o manipulador de eventos para o evento PreviewKeyDown
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;            
        }
        private void Previous_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listBoxSongs.Items.Count > 0)
                {
                    int selectedIndex = listBoxSongs.SelectedIndex;
                    int previousIndex = (selectedIndex - 1 + listBoxSongs.Items.Count) % listBoxSongs.Items.Count; // Calcula o índice da música anterior, garantindo que seja circular
                    listBoxSongs.SelectedIndex = previousIndex;
                }
                else
                {
                    MessageBox.Show("No songs available.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting previous song: {ex.Message}");
            }
        }

        private void Next_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listBoxSongs.Items.Count > 0)
                {
                    int selectedIndex = listBoxSongs.SelectedIndex;
                    int nextIndex = (selectedIndex + 1) % listBoxSongs.Items.Count; // Calcula o índice da próxima música, garantindo que seja circular
                    listBoxSongs.SelectedIndex = nextIndex;
                }
                else
                {
                    MessageBox.Show("No songs available.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting next song: {ex.Message}");
            }
        }

        private void PlayButton_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listBoxSongs.Items.Count > 0)
                {
                    if (listBoxSongs.SelectedItem == null)
                    {
                        listBoxSongs.SelectedIndex = 0; // Seleciona a primeira música da lista
                    }
                    mediaElement.Play();
                    isPlaying = true;
                    UpdatePlaybackIcons();
                }
                else
                {
                    MessageBox.Show("No songs available to play.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing media: {ex.Message}");
            }
        }

        private void PauseButton_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                mediaElement.Pause();
                isPlaying = false;
                UpdatePlaybackIcons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pausing media: {ex.Message}");
            }
        }
        private void UpdatePlaybackIcons()
        {
            playButton.Visibility = isPlaying ? Visibility.Collapsed : Visibility.Visible;
            pauseButton.Visibility = isPlaying ? Visibility.Visible : Visibility.Collapsed;
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = e.NewValue;
        }
        private void MuteButton_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                mediaElement.IsMuted = !mediaElement.IsMuted;
                isMuted = true;
                UpdateMutekIcons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error muting media: {ex.Message}");
            }
        }

        private void PlaySoundButton_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                mediaElement.IsMuted = !mediaElement.IsMuted;
                isMuted = false;
                UpdateMutekIcons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unmuting media: {ex.Message}");
            }
        }

        private void UpdateMutekIcons()
        {
            MuteButton.Visibility = isMuted ? Visibility.Collapsed : Visibility.Visible;
            PlaySoundButton.Visibility = isMuted ? Visibility.Visible : Visibility.Collapsed;
        }
        private void LoadSongs()
        {
            try
            {
                string directoryPath = "C:\\Users\\Marcos\\Documents\\extensao\\Dev_interface\\Musics";
                string[] songFiles = Directory.GetFiles(directoryPath);

                foreach (string songFile in songFiles)
                {
                     listBoxSongs.Items.Add(System.IO.Path.GetFileName(songFile));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}");
            }
        }
        private void listBoxSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             try
            {
                string? selectedSong = listBoxSongs.SelectedItem as string;
                if (selectedSong != null)
                {
                    string directoryPath = @"C:\Users\Marcos\Documents\extensao\Dev_interface\Musics";
                    string songPath = System.IO.Path.Combine(directoryPath, selectedSong);

                    mediaElement.Source = new Uri("file://" + songPath);
                    mediaElement.Play();
                    isPlaying = true;
                    UpdatePlaybackIcons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void LoadVideos()
        {
            try
            {
                string directoryPath = "C:\\Users\\Marcos\\Documents\\extensao\\Dev_interface\\Videos";
                string[] videoFiles = Directory.GetFiles(directoryPath);

                foreach (string videoFile in videoFiles)
                {
                    listBoxVideos.Items.Add(System.IO.Path.GetFileName(videoFile));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading videos: {ex.Message}");
            }
        }

        private void listBoxVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             try
            {
                string? selectedVideo = listBoxVideos.SelectedItem as string;
                if (selectedVideo != null)
                {
                    string directoryPath = @"C:\Users\Marcos\Documents\extensao\Dev_interface\Videos";
                    string videoPath = System.IO.Path.Combine(directoryPath, selectedVideo);

                    if (mediaElement.Parent != null)
                    originalParent = mediaElement.Parent as Panel;

                    mediaElement.Source = new Uri("file://" + videoPath);
                    mediaElement.Play();
                    isPlaying = true;
                    UpdatePlaybackIcons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Inicio_Click(object sender, MouseButtonEventArgs e)
        {
            Menu.Visibility = Visibility.Visible;
            Inicio.Visibility = Visibility.Visible;
            List_Music.Visibility = Visibility.Collapsed;            
            List_Video.Visibility = Visibility.Collapsed;
        }
        private void Music_Click(object sender, MouseButtonEventArgs e)
        {
            List_Music.Visibility = Visibility.Visible;         
            List_Video.Visibility = Visibility.Collapsed;
        }
        private void Video_Click(object sender, MouseButtonEventArgs e)
        {
            List_Video.Visibility = Visibility.Visible;
            List_Music.Visibility = Visibility.Collapsed;
        }
        private void Menu_Click(object sender, MouseButtonEventArgs e)
        {
            Menu.Visibility = Visibility.Visible;
            List_Music.Visibility = Visibility.Collapsed;            
            List_Video.Visibility = Visibility.Collapsed;
        }

        private void ShowFloatingMediaWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verifica se mediaElement não é nulo antes de exibi-lo em uma janela flutuante
                if (mediaElement != null)
                {
                    originalParent = mediaElement.Parent as Panel; // Armazena o pai original do mediaElement

                    if (originalParent != null)
                        originalParent.Children.Remove(mediaElement);

                    var floatingWindow = new FloatingMediaWindow
                    {
                        MediaElementToDisplay = mediaElement
                    };

                    floatingWindow.Closed += (s, args) =>
                    {
                        // Quando a janela flutuante for fechada, adiciona o mediaElement de volta ao seu pai original (grid)
                        if (originalParent != null) // Verifica se originalParent não é nulo antes de adicionar o mediaElement de volta a ele
                        {
                            originalParent.Children.Add(mediaElement);
                        }
                        else
                        {
                            // Caso originalParent seja nulo, você pode lidar com isso aqui, se necessário.
                            // Por exemplo, você pode escolher adicionar o mediaElement a um pai alternativo ou ignorar a operação.
                            MessageBox.Show("O pai original do MediaElement não foi armazenado corretamente.");
                        }
                    };

                    floatingWindow.Show();
                }
                else
                {
                    // Trata a situação em que mediaElement é nulo
                    MessageBox.Show("O MediaElement não está configurado ou é nulo.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Trata a exceção de InvalidOperationException que pode ocorrer ao exibir a janela flutuante
                MessageBox.Show($"Ocorreu um erro ao exibir a janela flutuante: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Trata outras exceções não previstas
                MessageBox.Show($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
         private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isFullScreen)
            {
                // Armazena a posição e o tamanho original do vídeo antes de maximizar em tela cheia
                previousVideoPosition = new Rect(mediaElement.Margin.Left, mediaElement.Margin.Top, mediaElement.Width, mediaElement.Height);

                // Remove o mediaElement do seu pai atual
                originalParent = mediaElement.Parent as Panel; // Armazena o pai original do mediaElement
                if (originalParent != null)
                    originalParent.Children.Remove(mediaElement);

                // Adiciona o mediaElement diretamente ao conteúdo da janela para permitir tela cheia
                Content = mediaElement;

                // Maximiza a janela em tela cheia
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;

                // Altera a variável de controle para indicar que está em tela cheia
                isFullScreen = true;
            }
            else
            {
                // Sai da tela cheia e volta ao tamanho normal
                ExitMediaElementFullScreen();
            }
        }
            private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Verifica se a tecla pressionada é a tecla "Esc"
            if (e.Key == Key.Escape)
            {
                // Sai da tela cheia e volta ao tamanho normal
                ExitMediaElementFullScreen();
            }
        }

        private void ExitMediaElementFullScreen()
        {
            if (isFullScreen)
            {
                // Restaura o mediaElement para o tamanho original
                mediaElement.Margin = new Thickness(previousVideoPosition.Left, previousVideoPosition.Top, 0, 0);
                mediaElement.Width = previousVideoPosition.Width;
                mediaElement.Height = previousVideoPosition.Height;

                // Volta o mediaElement ao seu pai original (grid)
                if (originalParent != null)
                    originalParent.Children.Add(mediaElement);
                else
                {
                    // Caso originalParent seja nulo, você pode lidar com isso aqui, se necessário.
                    // Por exemplo, você pode escolher adicionar o mediaElement a um pai alternativo ou ignorar a operação.
                    MessageBox.Show("O pai original do MediaElement não foi armazenado corretamente.");
                }

                // Altera a variável de controle para indicar que não está mais em tela cheia
                isFullScreen = false;
            }
        }
    }
}

