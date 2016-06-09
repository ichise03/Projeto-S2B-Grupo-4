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
    [Activity (Label = "Lan�amentos")]

    class telalancamentos : Activity
    {
        List<Lancamentos> DADOS = new List<Lancamentos>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telalancamentos);

            LoadList();

            FindViewById<ListView>(Resource.Id.contasView).ItemClick += List_ItemClick;           

            //Ativa o bot�o de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            //Chamada para a nova tela do bot�o gerar lan�amento
            FindViewById(Resource.Id.criarLancamento).Click += NovoLancamento;
        }

        public void LoadList()
        {
            //Implementar
        }

        //Fun��o para startar a nova tela
        void NovoLancamento(object sender, EventArgs e)
        {
            StartActivity(typeof(telagerarlancamento));
        }

        //Fun��o que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Implementar
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