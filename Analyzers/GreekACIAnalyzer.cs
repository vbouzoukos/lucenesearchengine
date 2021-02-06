﻿using System;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using System.IO;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Analysis.El;
using ParrotLucene.Analyzers.Filter;

namespace ParrotLucene.Analyzers
{
    public sealed class GreekACIAnalyzer : StopwordAnalyzerBase
    {
        /// <summary>
        /// File containing default Greek stopwords. </summary>
        public const string DEFAULT_STOPWORD_FILE = "stopwords.txt";

        /// <summary>
        /// Returns a set of default Greek-stopwords </summary>
        /// <returns> a set of default Greek-stopwords  </returns>
        public static CharArraySet DefaultStopSet => DefaultSetHolder.DEFAULT_SET;

        private class DefaultSetHolder
        {
            internal static readonly CharArraySet DEFAULT_SET = LoadDefaultSet();

            private static CharArraySet LoadDefaultSet() // LUCENENET: Avoid static constructors (see https://github.com/apache/lucenenet/pull/224#issuecomment-469284006)
            {
                try
                {
                    return LoadStopwordSet(false, typeof(GreekAnalyzer), DEFAULT_STOPWORD_FILE, "#");
                }
                catch (IOException ex)
                {
                    // default set should always be present as it is part of the
                    // distribution (JAR)
                    throw new Exception("Unable to load default stopword set", ex);
                }
            }
        }

        /// <summary>
        /// Builds an analyzer with the default stop words. </summary>
        /// <param name="matchVersion"> Lucene compatibility version,
        ///   See <see cref="LuceneVersion"/> </param>
        public GreekACIAnalyzer(LuceneVersion matchVersion)
              : this(matchVersion, DefaultSetHolder.DEFAULT_SET)
        {
        }

        /// <summary>
        /// Builds an analyzer with the given stop words. 
        /// <para>
        /// <b>NOTE:</b> The stopwords set should be pre-processed with the logic of 
        /// <see cref="GreekLowerCaseFilter"/> for best results.
        ///  
        /// </para>
        /// </summary>
        /// <param name="matchVersion"> Lucene compatibility version,
        ///   See <see cref="LuceneVersion"/> </param>
        /// <param name="stopwords"> a stopword set </param>
        public GreekACIAnalyzer(LuceneVersion matchVersion, CharArraySet stopwords)
              : base(matchVersion, stopwords)
        {
        }

        /// <summary>
        /// Creates
        /// <see cref="TokenStreamComponents"/>
        /// used to tokenize all the text in the provided <see cref="TextReader"/>.
        /// </summary>
        /// <returns> <see cref="TokenStreamComponents"/>
        ///         built from a <see cref="StandardTokenizer"/> filtered with
        ///         <see cref="GreekLowerCaseFilter"/>, <see cref="StandardFilter"/>,
        ///         <see cref="StopFilter"/>, and <see cref="GreekStemFilter"/> </returns>
        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            Tokenizer source = new StandardTokenizer(m_matchVersion, reader);
            TokenStream result = new GreekLowerCaseFilter(m_matchVersion, source);
            if (m_matchVersion.OnOrAfter(LuceneVersion.LUCENE_48))
            {
                result = new StandardFilter(m_matchVersion, result);
            }
            result = new StopFilter(m_matchVersion, result, m_stopwords);
            if (m_matchVersion.OnOrAfter(LuceneVersion.LUCENE_48))
            {
                result = new GreekStemFilter(result);
            }
            result = new GreekPhoneticFilter(result);
            result = new GreekAccentFilter(result);
            return new TokenStreamComponents(source, result);
        }
    }
}
