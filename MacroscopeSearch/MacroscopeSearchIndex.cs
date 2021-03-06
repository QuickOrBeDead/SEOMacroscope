﻿/*

	This file is part of SEOMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;

namespace SEOMacroscope
{

  /// <summary>
  /// Description of MacroscopeSearchIndex.
  /// </summary>

  public class MacroscopeSearchIndex : Macroscope
  {

    /**************************************************************************/

    public enum SearchMode
    {
      OR = 1,
      AND = 2
    }

    // Url, MacroscopeDocument
    private Dictionary<string,MacroscopeDocument> DocumentIndex;

    // Url, InvertedIndex ( sKeyword, Boolean )
    private Dictionary<string,Dictionary<string,Boolean>> ForwardIndex;

    // Url, DocumentIndex
    private Dictionary<string,Dictionary<string,MacroscopeDocument>> InvertedIndex;

    /**************************************************************************/

    public MacroscopeSearchIndex ()
    {

      this.SuppressDebugMsg = true;

      this.DocumentIndex = new Dictionary<string, MacroscopeDocument> ( 4096 );

      this.ForwardIndex = new Dictionary<string,Dictionary<string,Boolean>> ( 4096 );

      this.InvertedIndex = new Dictionary<string, Dictionary<string,MacroscopeDocument>> ( 4096 );

    }

    /**************************************************************************/

    public void AddDocumentToIndex ( MacroscopeDocument msDoc )
    {

      this.RemoveDocumentFromIndex( msDoc );

      this.ProcessText( msDoc );

    }

    /**************************************************************************/

    private void ProcessText ( MacroscopeDocument msDoc )
    {

      List<string> TextBlocks = new List<string> ( 16 );
      List<string> Terms = new List<string> ( 256 );

      TextBlocks.Add( msDoc.GetTitle() );
      TextBlocks.Add( msDoc.GetDescription() );
      TextBlocks.Add( msDoc.GetKeywords() );
      TextBlocks.Add( msDoc.GetBodyText() );

      DebugMsg( string.Format( "ProcessText: TextBlocks.Count: {0}", TextBlocks.Count ) );

      if( TextBlocks.Count > 0 )
      {
        for( int i = 0 ; i < TextBlocks.Count ; i++ )
        {
          string [] Chunk = TextBlocks[ i ].Split( ' ' );
          if( Chunk.Length > 0 )
          {
            for( int j = 0 ; j < Chunk.Length ; j++ )
            {
              if( Chunk[ j ].Length > 0 )
              {
                if( !Terms.Contains( Chunk[ j ] ) )
                {
                  Terms.Add( Chunk[ j ] );
                }
              }
            }
          }
        }
      }

      DebugMsg( string.Format( "ProcessText: Words :: {0}", Terms.Count ) );

      for( int i = 0 ; i < Terms.Count ; i++ )
      {

        string sTerm = Terms[ i ];

        Dictionary<string,MacroscopeDocument> DocumentReference;

        if( InvertedIndex.ContainsKey( sTerm ) )
        {
          DocumentReference = this.InvertedIndex[ sTerm ];
        }
        else
        {
          DocumentReference = new Dictionary<string,MacroscopeDocument> ();
          this.InvertedIndex.Add( sTerm, DocumentReference );
        }

        if( !DocumentReference.ContainsKey( msDoc.GetUrl() ) )
        {
          DocumentReference.Add( msDoc.GetUrl(), msDoc );
        }

      }

    }

    /**************************************************************************/

    private void RemoveDocumentFromIndex ( MacroscopeDocument msDoc )
    {
    }

    /** SEARCH INDEX **********************************************************/

    public List<MacroscopeDocument> ExecuteSearchForDocuments (
      MacroscopeSearchIndex.SearchMode SMode,
      string [] Terms
    )
    {
      List<MacroscopeDocument> DocList = null;
      switch( SMode )
      {
        case MacroscopeSearchIndex.SearchMode.OR:
          DocList = this.ExecuteSearchForDocumentsOR( Terms );
          break;
        case MacroscopeSearchIndex.SearchMode.AND:
          DocList = this.ExecuteSearchForDocumentsAND( Terms );
          break;
      }
      return( DocList );
    }

    /** SEARCH INDEX: OR METHOD ***********************************************/

    public List<MacroscopeDocument> ExecuteSearchForDocumentsOR ( string [] Terms )
    {

      List<MacroscopeDocument> DocList = new List<MacroscopeDocument> ();

      for( int i = 0 ; i < Terms.Length ; i++ )
      {

        if( InvertedIndex.ContainsKey( Terms[ i ] ) )
        {

          foreach( string Url in InvertedIndex[Terms[i]].Keys )
          {
            DocList.Add( InvertedIndex[ Terms[ i ] ][ Url ] );
          }

        }

      }

      return( DocList );

    }

    /** SEARCH INDEX: AND METHOD **********************************************/

    public List<MacroscopeDocument> ExecuteSearchForDocumentsAND ( string [] Terms )
    {

      List<MacroscopeDocument> DocList = new List<MacroscopeDocument> ();

      Dictionary<MacroscopeDocument,int> DocListGather = new Dictionary<MacroscopeDocument,int> ();

      for( int i = 0 ; i < Terms.Length ; i++ )
      {

        if( InvertedIndex.ContainsKey( Terms[ i ] ) )
        {

          foreach( string Url in InvertedIndex[Terms[i]].Keys )
          {

            MacroscopeDocument msDoc = InvertedIndex[ Terms[ i ] ][ Url ];
            if( DocListGather.ContainsKey( msDoc ) )
            {
              DocListGather[ msDoc ] = DocListGather[ msDoc ] + 1;
            }
            else
            {
              DocListGather.Add( msDoc, 1 );
            }

          }

        }

      }

      foreach( MacroscopeDocument msDoc in DocListGather.Keys )
      {
        if( DocListGather[ msDoc ] == Terms.Length )
        {
          DocList.Add( msDoc );
        }
      }

      return( DocList );

    }

    /**************************************************************************/

  }

}
