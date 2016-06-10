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
    [Activity(Label = "Gerar Lançamento")]

    class telagerarlancamento : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telagerarlancamento);

            //Ativa o botão de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			List<string> nomes = new List<string>();
			List<Contas> list = GerenciadorBanco.acessarContas();

			list.ForEach((Contas c) => {
				nomes.Add(c.Nome);
			});

			ArrayAdapter ad = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes);
			FindViewById<Spinner>(Resource.Id.spinner1).Adapter = ad;

			List<string> nomes2 = new List<string>();
			List<Categorias> list2 = GerenciadorBanco.acessarCategorias();

			list2.ForEach((Categorias c) => {
				nomes2.Add(c.Nome);
			});

			ArrayAdapter ad2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes2);
			FindViewById<Spinner>(Resource.Id.spinner3).Adapter = ad2;

			FindViewById<Button>(Resource.Id.button1).Click += Lancar;
		}

		public void Lancar (object sender, EventArgs e) {
			GerenciadorBanco.adicionarLancamento(2, 3, 5, 300, TipoLancamento.Debitar, new DateTime(2016, 03, 15), "Parabéns a todos. Hoje foi produtivo!");
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