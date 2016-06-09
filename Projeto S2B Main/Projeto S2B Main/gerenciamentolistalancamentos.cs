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
    class gerenciamentolistalancamentos : BaseAdapter<Lancamentos>
    {
        List<Lancamentos> DADOS;        

        Activity C;

        public gerenciamentolistalancamentos(List<Lancamentos> dados, Activity c)
        {
            DADOS = dados;            
            C = c;
        }

        public override int Count
        {
            get
            {
                return DADOS.Count;
            }
        }

        public override Lancamentos this[int position]
        {
            get
            {
                return DADOS[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = C.LayoutInflater.Inflate(Resource.Layout.celulalancamentos, null);            

            view.FindViewById<TextView>(Resource.Id.textContaRelacionada).Text = GerenciadorBanco.acessarConta(DADOS[position].ID_Conta).Nome;
            view.FindViewById<TextView>(Resource.Id.textData).Text = DADOS[position].Data_Hora.ToString();
            view.FindViewById<TextView>(Resource.Id.textValor).Text = "Saldo:" +  DADOS[position].Valor.ToString();

            return view;
        }
    }
}