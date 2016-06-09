using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Projeto_S2B_Main
{
    [Activity(Label = "Categorias")]

    class telacategorias : Activity
    {
        List<Categorias> DADOS = new List<Categorias>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telacategorias);

            LoadList();

            FindViewById<ListView>(Resource.Id.categoriasView).ItemClick += List_ItemClick;

            //Ativa o botão de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById(Resource.Id.criarCategoria).Click += NovaCategoria;
        }

        public void LoadList()
        {
            //Aqui serão adicionados os dados do DB
            DADOS = GerenciadorBanco.acessarCategorias();

            //Criando a listview e passando os parâmetros
            List<string> nomes = new List<string>();
            DADOS.ForEach((Categorias i) => {
                nomes.Add(i.Nome);
            });

            GerenciamentoLista GL = new GerenciamentoLista(nomes, this);

            FindViewById<ListView>(Resource.Id.categoriasView).Adapter = GL;
        }

        //Função para chamar a nova tela de criar categoria
        void NovaCategoria(object sender, EventArgs e)
        {
            StartActivity(typeof(telacriarcategoria));
        }

        //Função que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(telacriarcategoria));

            intent.PutExtra("isUpdate", true);
            intent.PutExtra("categoriaID", DADOS[e.Position].ID);
            intent.PutExtra("categoriaNome", DADOS[e.Position].Nome);
            intent.PutExtra("categoriaGrupo", DADOS[e.Position].Grupo);            

            StartActivity(intent);
        }

        //Função que faz o botão de voltar da action bar funcionar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}