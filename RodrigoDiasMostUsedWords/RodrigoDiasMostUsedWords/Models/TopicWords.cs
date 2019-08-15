using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RodrigoDiasMostUsedWords.Models
{
    public class TopicWords
    {
       private string topicDescription;
        public string TopicDescription { get => topicDescription; set => topicDescription = value; }

        private int quantityWords;
        public int QuantityWords { get => quantityWords; set => quantityWords = value; }


        public List<TopicWords> ListTopicWords()
        {

            Auxiliar auxiliar = new Auxiliar();

            rss outputXML = auxiliar.GetTopics();

            List<TopicWords> result = new List<TopicWords>();

            foreach (rssChannelItem rssi in outputXML.channel.item.Take(10)  )
            {
                result.Add(new TopicWords { TopicDescription = auxiliar.DecodeHtml( rssi.description), QuantityWords = auxiliar.GetWords(rssi.description + rssi.encoded).Count() });   
            }

            //string[] words = auxiliar.GetAllWords(outputXML);
            return result;
        }





    }
}