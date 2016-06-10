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

        protected override void OnCreate (Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telalancamentos);

            GerenciadorBanco.adicionarLancamento(2, 3, 5, 300, TipoLancamento.Debitar, new DateTime(2016, 03, 15), "Parab�ns a todos. Hoje foi produtivo!");

            //Aqui ser�o adicionados os dados do DB
            LoadList();
            FindViewById<ListView>(Resource.Id.lancamentosView).ItemClick += List_ItemClick;

            //Ativa o bot�o de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            //Chamada para a nova tela do bot�o gerar lan�amento
            FindViewById(Resource.Id.criarLancamento).Click += NovoLancamento;
        }

        public void LoadList () {
            //Aqui ser�o adicionados os dados do DB

            DADOS = GerenciadorBanco.acessarLancamento();

            gerenciamentolistalancamentos GL = new gerenciamentolistalancamentos(DADOS, this);
            FindViewById<ListView>(Resource.Id.lancamentosView).Adapter = GL;
        }
        //Fun��o para startar a nova tela
        void NovoLancamento(object sender, EventArgs e)
        {
            StartActivity(typeof(telagerarlancamento));
        }

        //Fun��o que define o que acontece quando clica no item da listview
        void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Implementar. Utilizar gerenciamentolistalancamentos
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