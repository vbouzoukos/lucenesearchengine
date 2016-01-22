﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearchEngine
{
    public enum SearchFieldOption {TERM,LIKE,INTRANGE,DOUBLERANGE }
    public class SearchTerm
    {
        /// <summary>
        /// The term Field
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Term Occurance in search
        /// </summary>
        public Lucene.Net.Search.Occur TermOccur {get; set;}
        /// <summary>
        /// The Query Term
        /// </summary>
        public string Term { get; set; }
        /// <summary>
        /// The Term is a range
        /// </summary>
        public SearchFieldOption SearchingOption { get; set; }
        /// <summary>
        /// Starting Range of the term
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Ending Range of the Term
        /// </summary>
        public string To { get; set; }

        public int? iFrom
        {
            get
            {
                int ret = 0;
                int.TryParse(From, out ret);
                return ret > 0 ? new int?(ret) : null;
            }
        }
        public int? iTo
        {
            get
            {
                int ret = 0;
                int.TryParse(To, out ret);
                return ret > 0 ? new int?(ret) : null;
            }
        }
        public double? dFrom
        {
            get
            {
                int ret = 0;
                int.TryParse(From, out ret);
                return ret > 0 ? new int?(ret) : null;
            }
        }
        public double? dTo
        {
            get
            {
                int ret = 0;
                int.TryParse(To, out ret);
                return ret > 0 ? new int?(ret) : null;
            }
        }
        public SearchTerm(string field,string term="",SearchFieldOption option=SearchFieldOption.TERM, Lucene.Net.Search.Occur toccur= Lucene.Net.Search.Occur.MUST)
        {
            Field = field;
            TermOccur = toccur;
            Term = term;
            SearchingOption = option;
        }
        public SearchTerm(string field, DateTime dFrom, DateTime dTo, Lucene.Net.Search.Occur toccur = Lucene.Net.Search.Occur.MUST)
        {
            Field = field;
            From = Utility.DateSerialize(dFrom);
            To = Utility.DateSerialize(dTo);
            TermOccur = toccur;
            SearchingOption = SearchFieldOption.INTRANGE;
        }
        public SearchTerm(string field, int from, int to, Lucene.Net.Search.Occur toccur = Lucene.Net.Search.Occur.MUST)
        {
            Field = field;
            From = from.ToString();
            To = to.ToString();
            TermOccur = toccur;
            SearchingOption = SearchFieldOption.INTRANGE;
        }
        public SearchTerm(string field, double from, double to, Lucene.Net.Search.Occur toccur = Lucene.Net.Search.Occur.MUST)
        {
            Field = field;
            From = from.ToString();
            To = to.ToString();
            TermOccur = toccur;
            SearchingOption = SearchFieldOption.DOUBLERANGE;
       }
    }
}
