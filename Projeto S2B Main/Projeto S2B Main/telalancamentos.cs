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
    [Activity (Label = "Lançamentos")]

    class telalancamentos : Activity
    {
        List<Lancamentos> DADOS = new List<Lancamentos>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telalancamentos);

            LoadList();

            FindViewById<ListView>(Resource.Id.contasView).ItemClick += List_ItemClick;           

            //Ativa o botão de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            //Chamada para a nova tela do botão gerar lançamento
            FindViewById(Resource.Id.criarLancamento).Click += NovoLancamento;
        }

        public void LoadList()
        {
            //Implementar
        }

        //Função para startar a nova tela
        void NovoLancamento(object sender, EventArgs e)
        {
            StartActivity(typeof(telagerarlancamento));
        }

        //Função que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Implementar
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