using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace RodrigoDiasMostUsedWords.Models
{
    public class Auxiliar
    {
        public T DeSerializeXMLOnline<T>()
        {
            T outputXML = default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            string url = ConfigurationManager.AppSettings["XmlLink"];
            
           
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    outputXML = (T)serializer.Deserialize(reader);

                }                
            

            return outputXML;
        }

        public T DeSerializeXMLLocal<T>()
        {
            T outputXML = default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string filepath = ConfigurationManager.AppSettings["XmlPath"];
            StreamReader reader = new StreamReader(filepath);
            outputXML = (T)serializer.Deserialize(reader);
            reader.Close();

            return outputXML;
        }

        public string[] GetAllWords(rss inputXML)

        {
            // Obtendo todos os textos 
            var all = string.Join("", inputXML.channel.item.Select(x => x.encoded + x.description));            

            var words = this.GetWords(all);

            return words.ToArray();
        }



        public string[] GetWords(string encoded)

        {

            //Preposições e artigos
            string[] prepos = { "e", "é", "o", "os", "a", "as", "um", "uns", "uma", "umas", "a", "ao", "aos", "à", "às", "de", "do", "dos", "da", "das", "dum", "duns", "duma", "dumas", "em", "no", "nos", "na", "nas", "num", "nuns", "numa", "numas", "por", "pelo", "pelos", "pela", "pelas", "Que", "Ante", "Após", "Até", "Com", "Contra", "De", "Desde", "Em", "Entre", "Para", "Per", "Perante", "Por", "Sem", "Sob", "Sobre", "Trás", "Afora", "Conforme", "Consoante", "Durante", "Exceto", "Salvo", "Malgrado", "Ao", "Aos", "Aonde", "Do", "No", "Dum", "Num", "Desta", "Neste", "Pelo", "Pelas", "Dalguma", "Doutro" };

            MatchCollection matches = this.DecodeHtmlToMatch(encoded);

            //Minerar as palavras
            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        && !prepos.Any(pre => m.Value.ToUpper() == pre.ToUpper())
                        select (TrimSuffix(m.Value));


            return words.ToArray();
        }

        public MatchCollection DecodeHtmlToMatch(string htmlText)
        {
                        
            MatchCollection matches = Regex.Matches(this.DecodeHtml(htmlText), @"\b[\w']*\b");

            return matches;
        }


        public string DecodeHtml(string htmlText)
        {

            // Removendo tags html e pontuação
            htmlText = HttpUtility.HtmlDecode(htmlText);
            htmlText = Regex.Replace(htmlText, "<.*?>", String.Empty);
            

            return htmlText;
        }




        public string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }


        public rss GetTopics()
        {
            rss outputXML = new rss();
            Auxiliar auxiliar = new Auxiliar();

            try
            {

                outputXML= auxiliar.DeSerializeXMLOnline<rss>();
            }
            catch(Exception ex)
            {
                outputXML = auxiliar.DeSerializeXMLLocal<rss>();
                
            }
            

//            string path = "";
            

            

             

            return outputXML;
        }

    }
}