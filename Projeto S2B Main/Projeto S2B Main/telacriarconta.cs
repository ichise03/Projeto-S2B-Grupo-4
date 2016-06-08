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
    [Activity(Label = "Nova Conta")]

    class telacriarconta : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telacriarconta);

            //Ativa o botão de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById(Resource.Id.button1).Click += CriarConta;
        }

        void CriarConta(object sender, EventArgs e)
        {
            gerenciadorBanco gb = new gerenciadorBanco();

            Contas.TipoConta tipo = new Contas.TipoConta();

            if (FindViewById(Resource.Id.radioButton1).Hovered == true)
            {
                tipo = Contas.TipoConta.Moeda_Em_Espécie;
            }
            else if (FindViewById(Resource.Id.radioButton2).Hovered == true)
            {
                tipo = Contas.TipoConta.Cartão_De_Crédito;
            }
            else if (FindViewById(Resource.Id.radioButton3).Hovered == true)
            {
                tipo = Contas.TipoConta.Cartão_De_Débito;
            }
            else if (FindViewById(Resource.Id.radioButton4).Hovered == true)
            {
                tipo = Contas.TipoConta.Poupança;
            }
            
            gb.adicionarConta(FindViewById<EditText>(Resource.Id.editText1).Text, decimal.Parse(FindViewById<EditText>(Resource.Id.editText2).Text), tipo);            
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