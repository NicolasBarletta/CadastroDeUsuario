using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cadastro_De_Usuario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.usuariosDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'usuariosDataSet.Table'. Você pode movê-la ou removê-la conforme necessário.
            this.tableTableAdapter.Fill(this.usuariosDataSet.Table);

        }

        private void localizar_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cepTextBox.Text + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Erro na requisição: " + ChecaServidor.StatusCode.ToString());
                return; // Encerra o códigoz
            }

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        String response = responseReader.ReadToEnd();
                        MessageBox.Show(response);
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");
                        MessageBox.Show(response);

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            // CEP
                            if (cont == 1)
                            {
                                string[] valor = substring.Split(':');
                                cepTextBox.Text = valor[1].ToString();
                            }

                            // Logradouro
                            if (cont == 2)
                            {
                                string[] valor = substring.Split(':');
                                ruaTextBox.Text = valor[1].ToString();
                            }


                            // Bairro
                            if (cont == 4)
                            {
                                string[] valor = substring.Split(':');
                                bairroTextBox.Text = valor[1].ToString();
                            }

                            // Cidade
                            if (cont == 5)
                            {
                                string[] valor = substring.Split(':');
                                cidadeTextBox.Text = valor[1].ToString();
                            }

                            // UF
                            if (cont == 6)
                            {
                                string[] valor = substring.Split(':');
                                estadoTextBox.Text = valor[1].ToString();
                            }
                            cont++;
                        }
                    }
                }
            
        }
    }

        private void emailTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;

            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(email))
            {
                string message = "E-mail válido!";
                string title = "Title";
                MessageBox.Show(message, title);
            }
            else
            {
                string message = "E-mail inválido!";
                string title = "Title";
                MessageBox.Show(message, title);
            }
        }
    }
}
