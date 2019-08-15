using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RodrigoDiasMostUsedWords.Models
{
    public class TrendWords
    {
        private string wordDescription;
        public string WordDescription { get => wordDescription; set => wordDescription = value; }

        private int quantityRecorrence;
        public int QuantityRecorrence { get => quantityRecorrence; set => quantityRecorrence = value; }



        public List<TrendWords> ListTrendWords()
        {

            Auxiliar auxiliar = new Auxiliar();

            rss outputXML = auxiliar.GetTopics();


            string[] words = auxiliar.GetAllWords(outputXML);

            List<TrendWords> listtopicWords = new List<TrendWords>();

            //Listando palavras
            foreach (string s in words)
            {
                listtopicWords.Add(new TrendWords { WordDescription = s, QuantityRecorrence= 1 });
            }

            //Agrupando as 10 palavras mais recorrentes 
            var result = listtopicWords.GroupBy(x => x.WordDescription.ToUpper()).OrderByDescending(res => res.Count()).Select(res => new TrendWords() { WordDescription = res.Key, QuantityRecorrence= res.Count() }).Take(10).ToList();

            return result;

        }


    }
}