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
    [Activity(Label = "Transfer�ncias")]
    class telatransferencias : Activity
    {
        List<string> DADOS = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telatransferencias);

            //Aqui ser�o adicionados os dados do DB
            DADOS.Add("Item 1");
            DADOS.Add("Item 2");
            DADOS.Add("Item 3");
            DADOS.Add("Item 4");

            //Criando a listview e passando os par�metros
            ListView List = FindViewById<ListView>(Resource.Id.tranferenciasView);

            GerenciamentoLista GL = new GerenciamentoLista(DADOS, this);

            List.Adapter = GL;
            List.ItemClick += List_ItemClick;

            //Ativa o bot�o de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            //Chamada para a fun��o que chama a nova tela
            FindViewById(Resource.Id.criarTransferencia).Click += NovaTransferencia;
        }

        //Fun��o que chama a nova tela
        void NovaTransferencia(object sender, EventArgs e)
        {
            StartActivity(typeof(telafazertransferencia));
        }

        //Fun��o que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, DADOS[e.Position], ToastLength.Short).Show();
        }

        //Fun��o que faz o bot�o de voltar da action bar funcionar
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