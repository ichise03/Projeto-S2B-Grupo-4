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
    [Activity (Label ="Contas")]

    class telacontas : Activity
    {
        List<string> DADOS = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telacontas);

            //Aqui ser�o adicionados os dados do DB
            gerenciadorBanco gb = new gerenciadorBanco();

            DADOS = (gb.acessarNomeContas());             

            //Criando a listview e passando os par�metros
            ListView List = FindViewById<ListView>(Resource.Id.listView1);

            GerenciamentoLista GL = new GerenciamentoLista(DADOS, this);

            List.Adapter = GL;
            List.ItemClick += List_ItemClick;

            //Ativa o bot�o de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById(Resource.Id.button1).Click += NovaConta;

        }

        //Fun��o para iniciar a activity de nova conta
        void NovaConta(object sender, EventArgs e)
        {
            StartActivity(typeof(telacriarconta));                     
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            StartActivity(typeof(telacontas));                  
        }

        //Fun��o que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ;
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