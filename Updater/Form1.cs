using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel;

namespace Updater
{
    public partial class Form1 : Form
    {
        #region DLL usadas
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        #endregion

        #region FTPItem
        public class FTPItem
        {
            public char[] Permissions { get; set; }
            public long Size { get; set; }
            public DateTime LastModified { get; set; }
            public string Name { get; set; }
            public string FullPath { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
        public class FTPDirectory : FTPItem
        {
            public FTPDirectory() { }
            public FTPDirectory(FTPItem item)
            {
                Permissions = item.Permissions;
                Size = item.Size;
                LastModified = item.LastModified;
                Name = item.Name;
                FullPath = item.FullPath;
            }

            public Lazy<List<FTPItem>> SubItems { get; set; }
        }
        public class FTPFile : FTPItem
        {
            public FTPFile() { }
            public FTPFile(FTPItem item)
            {
                Permissions = item.Permissions;
                Size = item.Size;
                LastModified = item.LastModified;
                Name = item.Name;
                FullPath = item.FullPath;
            }
        }
        #endregion

        #region Configurações

        static String Nome_Server = "L2jBrasil";
        private String Url_Discord = "https://discord.gg/fBDFYmPu";
        private String Url_Site = "https://discord.gg/fBDFYmPu";
        private String Url_Cadastro = "https://discord.gg/fBDFYmPu";
        static private String Caminho_Client = "C:\\Lineage2";
        private String Caminho_Client_l2_exe = "C:\\Lineage2\\system\\L2.exe";
        private String Path = "C:\\Lineage2\\updates\\" + Nome_Server + ".l2";
        private String Icone = @"C:\Lineage2\\icone.ico";

        private String Url_Ftp = "ftp://127.0.0.1";
        private String User_Ftp = "teste";
        private String Password_Ftp = "manager";
        private Boolean ExcluirTemporario = true;
        private long Arquivo_Size = 0;
        private readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        List<FTPItem> Arquivos = new List<FTPItem>();

        List<string> Atualizacoes_Concluidas = new List<string>();
        List<string> Atualizacoes_de_comparacao = new List<string>();
        List<string> ListaDeDownload = new List<string>();


        Process[] Processo = Process.GetProcessesByName("l2");
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        //// Menus
        private ContextMenu ContextMenu;
        private MenuItem Abrir_Menu;
        private MenuItem Atualizar_Menu;
        private MenuItem Discord_Menu;
        private MenuItem MinhaConta_Menu;
        private MenuItem Fechar_Menu;
        //////////

        #endregion

        #region Inicializador do Form
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            btnStart.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnStart.Width,
            btnStart.Height, 150, 150));
            btnAtualizar.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAtualizar.Width,
            btnAtualizar.Height, 150, 150));
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
            btnClose.Height, 150, 150));
            bar01.Width = 0;
            bar02.Width = 0;
            status.Text = "Iniciando updater....";
            velocidade.Text = "";
            porcentagem01.Text = "0%";
            porcentagem02.Text = "0%";

            #region Configurção dos menus
            Abrir_Menu = new MenuItem();
            Abrir_Menu.Index = 1;
            Abrir_Menu.Text = "Abrir";
            Abrir_Menu.Click += new System.EventHandler(Maximizar);

            Atualizar_Menu = new MenuItem();
            Atualizar_Menu.Index = 1;
            Atualizar_Menu.Text = "Atualizar";
            Atualizar_Menu.Click += new System.EventHandler(VereficandoSeExisteAtualizacaoClick);

            Discord_Menu = new MenuItem();
            Discord_Menu.Index = 2;
            Discord_Menu.Text = "Discord";
            Discord_Menu.Click += new System.EventHandler(AbrirDiscord);


            MinhaConta_Menu = new MenuItem();
            MinhaConta_Menu.Index = 3;
            MinhaConta_Menu.Text = "Minha Conta";
            MinhaConta_Menu.Click += new System.EventHandler(AbrirCadastro);

            Fechar_Menu = new MenuItem();
            Fechar_Menu.Index = 4;
            Fechar_Menu.Text = "Fechar";
            Fechar_Menu.Click += new System.EventHandler(FecharAplicacao);

            ContextMenu = new ContextMenu();
            ContextMenu.MenuItems.AddRange(new MenuItem[] { Abrir_Menu, Atualizar_Menu, Discord_Menu, MinhaConta_Menu, Fechar_Menu });
            #endregion

            if (!Directory.Exists(Caminho_Client))
            {
                Directory.CreateDirectory(Caminho_Client);
            };

            if (!Directory.Exists(Caminho_Client + "\\updates"))
            {
                Directory.CreateDirectory(Caminho_Client + "\\updates");
            };

            if (!File.Exists(Path))
            {
                var FilePath = File.CreateText(Path);
                FilePath.Close();
            };
            StreamReader PathArquive = new StreamReader(Path);
            while (PathArquive.Peek() >= 0)
            {
                String Arquivo = PathArquive.ReadLine();
                if (Atualizacoes_Concluidas.Contains(Arquivo) == false)
                {
                    Atualizacoes_Concluidas.Add(Arquivo);
                }
            }
            PathArquive.Close();
                     

            timer.Enabled = true;
            timer.Tick += new System.EventHandler(VereficandoSeExisteAtualizacao);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            NotifyIconInicialize();

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }
        #endregion

        #region Inicia o NotifyIcon
        public void NotifyIconInicialize()
        {
         NotifyIcon Notificacao = new NotifyIcon()
         {
                Icon = new Icon(Icone),
                Text = Nome_Server,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipText = "Mantenha seu client sempre atualizado",
                BalloonTipTitle = "Seja Bem Vindo ao " + Nome_Server,
                ContextMenu = ContextMenu

        };
            Notificacao.ShowBalloonTip(1000);
     
        }

        #endregion

        #region Maximizar Aplicação
        public void Maximizar(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            this.CenterToParent();
            this.CenterToScreen();


        }
        #endregion

        #region Minimizar Aplicação
        public void Minimizar(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }
        #endregion

        #region Fechar Aplicação
        public void FecharAplicacao(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }
        #endregion

        #region Abrir Dircord
        public void AbrirDiscord(object sender, EventArgs e)
        {
            Process.Start(Url_Discord);
        }
        #endregion

        #region Abrir Cadastro
        public void AbrirCadastro(object sender, EventArgs e)
        {
            Process.Start(Url_Cadastro);
        }
        #endregion

        #region Abrir Site
        public void AbrirSite(object sender, EventArgs e)
        {
            Process.Start(Url_Site);
        }
        #endregion

        #region Abrir Game
        public void AbrirGame(object sender, EventArgs e)
        {
            if (File.Exists(Caminho_Client_l2_exe))
            {
                var startInfo = new ProcessStartInfo(Caminho_Client_l2_exe);
                Process.Start(startInfo);
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Info,
                    BalloonTipText = string.Format("Bom jogo."),
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
            }
            else
            {
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Error,
                    BalloonTipText = string.Format("Não localizei o inicializador do Lineage 2, no caminho {0} , caso o cliente esteja desatualizado atualizeo.", Caminho_Client_l2_exe),
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
            }
          
        }
        #endregion

        #region Vereficando se existe atualização

        public void VereficandoSeExisteAtualizacaoClick(object sender, EventArgs e)
        {
            btnAtualizar.Enabled = false;
            btnStart.Enabled = false;
            status.Text = "Iniciando busca por atualizações..";
            bar01.Width = 0;
            porcentagem01.Text = "0%";

            timer.Enabled = true;
            timer.Tick += new System.EventHandler(VereficandoSeExisteAtualizacao);
        }
            public void VereficandoSeExisteAtualizacao(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Stop();

            if (Processo.Length > 0)
            {
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Error,
                    BalloonTipText = "Feche o game para atualizar.",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
                status.Text = "Feche o game para atualizar.";
                return;
            }


            try
            {
                btnAtualizar.Enabled = false;
                btnStart.Enabled = false;
                status.Text = "Iniciando busca por atualizações..";
                Arquivos.Clear();
                Atualizacoes_de_comparacao.Clear();
                status.Text = "Conectando com servidor ...";
                velocidade.Text = "";
                Thread.Sleep(3000);

                FtpWebRequest Request = (FtpWebRequest)WebRequest.Create(Url_Ftp);
                Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                Request.Credentials = new NetworkCredential(User_Ftp, Password_Ftp);
                Request.KeepAlive = false;
                Request.UseBinary = true;
                Request.UsePassive = true;

                Regex directoryListingRegex = new Regex(@"^([d-])((?:[rwxt-]{3}){3})\s+\d{1,}\s+.*?(\d{1,})\s+(\w+)\s+(\d{1,2})\s+(\d{4})?(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                                   RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                porcentagem01.Text = "0%";
                porcentagem02.Text = "0%";
                bar01.Width = 0;
                bar02.Width = 0;

                using (FtpWebResponse response = (FtpWebResponse)Request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {

                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Match match = directoryListingRegex.Match(line);
                                FTPItem item = new FTPItem
                                {
                                    Permissions = match.Groups[2].Value.ToArray(),
                                    Size = long.Parse(match.Groups[3].Value),
                                    LastModified = DateTime.ParseExact($"{match.Groups[4]} {match.Groups[5]} {match.Groups[6]} {match.Groups[7]}",
                                                   $"MMM dd {(match.Groups[6].Value != "" ? "yyyy" : "")} {(match.Groups[7].Value != "" ? "HH:mm" : "")}",
                                                   CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
                                    Name = match.Groups[8].Value,
                                    FullPath = $"{Url_Ftp}/{match.Groups[8].Value.Replace(" ", "%20")}",
                                };


                                if (match.Groups[1].Value == "d")
                                {
                                    FTPDirectory dir = new FTPDirectory(item);

                                    if (Arquivos.Contains(dir) == false)
                                    {
                                       // Arquivos.Add(dir);
                                    }
                                }
                                else
                                {
                                    FTPFile file = new FTPFile(item);

                                    if (Arquivos.Contains(file) == false)
                                    {
                                        Arquivos.Add(file);
                                    }
                                }

                            }

                            GravarAtualizacoes();
                            Vereficaebaixa();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Error,
                    BalloonTipText = "Não foi possivel conectar com o servidor de update.",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
                status.Text = "Não foi possivel conectar com o servidor de update.";
                velocidade.Text = "";
                btnAtualizar.Enabled = true;
                btnStart.Enabled = true;
            }

        }
        #endregion

        #region Pega a atualização mais recente
        private void GravarAtualizacoes()
        {
            foreach (FTPItem item in Arquivos)
            {
                Atualizacoes_de_comparacao.Add(item.Name);
            }
        }
        #endregion

        #region Vereficar e Baixar
        public async void Vereficaebaixa()
        {
            List<String> Lista = Atualizacoes_de_comparacao.Except(Atualizacoes_Concluidas).ToList();

            if (Lista.Count > 0)
            {
              
                foreach (String item in Lista)
                {
                    ListaDeDownload.Add(item);
                }
                stopwatch.Start();
                BaixarAtualizacao(ListaDeDownload[0]);
            }
            else
            {
                status.Text = "Cliente atualizado...";
                bar01.Width = 369;
                bar02.Width = 369;
                porcentagem01.Text = "100%";
                porcentagem02.Text = "100%";
                btnAtualizar.Enabled = true;
                btnStart.Enabled = true;
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Info,
                    BalloonTipText = "Opaaaa... Vamos jogar. Seu cliente esta atualizado",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
            };           
        }

        #endregion

        #region Descompactar
        private void Descompactar(object Sender, DoWorkEventArgs E)
        {
                string Path = E.Argument as string;

                try
            {
                using (var zipFile = new ZipFile(System.Windows.Forms.@Application.UserAppDataPath + Path))
                {
                    this.Invoke(new Action(() => labaelextrando.Text = "Extrando ..."));
                    zipFile.ExtractProgress +=
                        (o, args) =>
                        {
                            if (args.TotalBytesToTransfer > 0)
                            {
                                var percentage = Convert.ToInt32(100 * args.BytesTransferred / args.TotalBytesToTransfer);
                                int Barra = percentage * 369 / 100;
                                this.Invoke(new Action(() => bar02.Width = Barra));
                                this.Invoke(new Action(() => porcentagem02.Text = string.Format("{0}%", percentage)));
                                this.Invoke(new Action(() => labaelextrando.Text = args.CurrentEntry.ToString()));
                            }

                        };
                    zipFile.ExtractAll(Caminho_Client, ExtractExistingFileAction.OverwriteSilently);
                    if (ExcluirTemporario)
                    {
                        File.Delete(System.Windows.Forms.@Application.UserAppDataPath + Path);
                    };
                    this.Invoke(new Action(() => DownloadChangeComplet(Path)));
                }

            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => status.Text = "Falho ao descompactar atualização"));
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Error,
                    BalloonTipText = "Falho ao descompactar atualização",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
                if (ExcluirTemporario)
                {
                    File.Delete(System.Windows.Forms.@Application.UserAppDataPath + Path);
                };
                this.Invoke(new Action(() => btnAtualizar.Enabled = true));
                this.Invoke(new Action(() => btnStart.Enabled = true));
            }

        }
        #endregion

        #region Baixar Atualização
        private void BaixarAtualizacao(string Path)
        {

            if (File.Exists(System.Windows.Forms.@Application.UserAppDataPath + Path))
            {
                this.Invoke(new Action(() => status.Text = "Descompactando atualização"
                ));
                this.Invoke(new Action(() => porcentagem01.Text = "100%"
               ));
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Info,
                    BalloonTipText = "Descompactando atualização",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
                this.Invoke(new Action(() => bar01.Width = 369
              ));
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Descompactar;
                bw.RunWorkerAsync(Path);

            }
            else
            {

                FtpWebRequest request = FtpWebRequest.Create(Url_Ftp + "/" + Path) as FtpWebRequest;
                request.Credentials = new NetworkCredential(User_Ftp, Password_Ftp);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                Arquivo_Size = (long)request.GetResponse().ContentLength;
                using (WebClient Client = new WebClient())
                {
                    Client.Credentials = new NetworkCredential(User_Ftp, Password_Ftp);
                    ListaDeDownload.RemoveAt(0);
                    Client.DownloadProgressChanged += (sender, e) =>
                    {
                        string downloadProgress = string.Format("{0}%", e.BytesReceived * 100 / Arquivo_Size);
                        int Barra = (int)(e.BytesReceived * 100 / Arquivo_Size) * 369 / 100; //*369 / 100
                        this.Invoke(new Action(() => bar01.Width = Barra));
                        string downloadedMBs = SizeSuffix(e.BytesReceived);
                        string totalMBs = SizeSuffix(Arquivo_Size);
                        string progress = $"Baixando {Path} - {downloadedMBs}/{totalMBs}";
                        this.Invoke(new Action(() => velocidade.Text = string.Format("{0} MB/s", (e.BytesReceived / 1024.0 / 1024.0 / stopwatch.Elapsed.TotalSeconds).ToString("0.00"))));
                        this.Invoke(new Action(() => status.Font = new Font("Arial", 8) ));
                        this.Invoke(new Action(() => status.Text = progress));
                        this.Invoke(new Action(() => porcentagem01.Text = downloadProgress));

                    };
                    Client.DownloadFileCompleted += async (sender, e) =>
                    {
                        stopwatch.Stop();
                        stopwatch.Reset();
                        if (File.Exists(System.Windows.Forms.@Application.UserAppDataPath + Path))
                        {
                            this.Invoke(new Action(() => status.Text = "Descompactando atualização"));
                            this.Invoke(new Action(() => porcentagem01.Text = "100%"));
                            NotifyIcon Notificacaos = new NotifyIcon()
                            {
                                Icon = new Icon(Icone),
                                Text = Nome_Server,
                                Visible = true,
                                BalloonTipIcon = ToolTipIcon.Info,
                                BalloonTipText = "Descompactando atualização",
                                BalloonTipTitle = Nome_Server

                            };
                            Notificacaos.ShowBalloonTip(100);
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += Descompactar;
                            bw.RunWorkerAsync(Path);
                        }
                        else
                        {
                            this.Invoke(new Action(() => status.Text = "O Arquivo Zip não foi localizado"));
                            this.Invoke(new Action(() => btnAtualizar.Enabled = true));
                            this.Invoke(new Action(() => btnStart.Enabled = true));
                            NotifyIcon Notificacao = new NotifyIcon()
                            {
                                Icon = new Icon(Icone),
                                Text = Nome_Server,
                                Visible = true,
                                BalloonTipIcon = ToolTipIcon.Error,
                                BalloonTipText = "Não encontrei o arquivo da atualizaçã",
                                BalloonTipTitle = Nome_Server

                            };
                            Notificacao.ShowBalloonTip(100);
                        }

                    };
                    try
                    {
                        Client.DownloadFileAsync(new Uri(Url_Ftp + "/" + Path), System.Windows.Forms.@Application.UserAppDataPath + Path);
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => status.Font = new Font("Arial", 8)));
                        this.Invoke(new Action(() => status.Text = "Falha ao baixar atualização"));
                        this.Invoke(new Action(() => btnAtualizar.Enabled = true));
                        this.Invoke(new Action(() => btnStart.Enabled = true));
                        NotifyIcon Notificacao = new NotifyIcon()
                        {
                            Icon = new Icon(Icone),
                            Text = Nome_Server,
                            Visible = true,
                            BalloonTipIcon = ToolTipIcon.Error,
                            BalloonTipText = "Ouve uma falha ao baixar a atualização",
                            BalloonTipTitle = Nome_Server

                        };
                        Notificacao.ShowBalloonTip(100);
                    }

                }
            }
        }
        #endregion

        #region Atualiza Status do download

        static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
       
        #endregion

        #region Download completo
        private void DownloadChangeComplet(string FileName)
        {
            StreamWriter Anotar = new StreamWriter(Path, append: true);
            Anotar.Write(FileName);
            Anotar.WriteLine();
            Anotar.Close();

            if (ListaDeDownload.Count > 0)
                {
                stopwatch.Restart();
                BaixarAtualizacao(ListaDeDownload[0]);
                }
                else
                {
                status.Text = "Cliente atualizado";
                bar01.Width = 369;
                bar02.Width = 369;
                porcentagem01.Text = "100%";
                porcentagem02.Text = "100%";
                btnAtualizar.Enabled = true;
                btnStart.Enabled = true;
                NotifyIcon Notificacao = new NotifyIcon()
                {
                    Icon = new Icon(Icone),
                    Text = Nome_Server,
                    Visible = true,
                    BalloonTipIcon = ToolTipIcon.Info,
                    BalloonTipText = "Opaaaa... Vamos jogar. Seu cliente esta atualizado",
                    BalloonTipTitle = Nome_Server

                };
                Notificacao.ShowBalloonTip(100);
            }
        }
        #endregion

    }
}
