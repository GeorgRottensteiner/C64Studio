using System;
using System.Collections.Generic;
using System.Text;

namespace GR
{
  namespace Strings
  {
    public class XMLElement : IEnumerable<GR.Strings.XMLElement>
    {
      public class XMLAttribute
      {
        private string   name = "";

        private string content = "";

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Content
        {
          get
          {
            return content;
          }
          set
          {
            content = value;
          }
        }

        public XMLAttribute()
        {
        }

        public XMLAttribute( string name, string content )
        {
          this.name = name;
          this.content = content;
        }
      };

      private string type = "";
      private string content = "";

      protected List<XMLElement> m_Childs = new List<XMLElement>();

      private List<XMLAttribute> m_Attributes = new List<XMLAttribute>();


      public XMLElement Parent
      {
        get;
        set;
      }



      public List<XMLAttribute> Attributes
      {
        get
        {
          return m_Attributes;
        }
        set
        {
          m_Attributes = value;
        }
      }



      public XMLElement()
      {
        Parent = null;
      }

      public XMLElement( string type )
      {
        this.type = type;
        Parent = null;
      }

      public XMLElement( string type, string content )
      {
        this.type = type;
        this.content = content;
        Parent = null;
      }

      public string Type
      {
        get
        {
          return type;
        }
        set
        {
          type = value;
        }
      }

      public string Content
      {
        get
        {
          return content;
        }
        set
        {
          SetContent( value );
        }
      }

      public System.Collections.Generic.IEnumerator<XMLElement> GetEnumerator()
      {
        return m_Childs.GetEnumerator();
      }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
        return m_Childs.GetEnumerator();
      }

      public int GetChildCount()
      {
        return m_Childs.Count;
      }



      public string XMLEncode( string ToEncode )
      {
        string ReturnContent = ToEncode;

        // & muﬂ als erstes!
        ReturnContent = ReturnContent.Replace( "&", "&amp;" );
        ReturnContent = ReturnContent.Replace( "<", "&lt;" );
        ReturnContent = ReturnContent.Replace( ">", "&gt;" );
        ReturnContent = ReturnContent.Replace( "\'", "&apos;" );
        ReturnContent = ReturnContent.Replace( "\"", "&quot;" );

        return ReturnContent;
      }



      public string XMLDecode( string ToDecode )
      {
        string ReturnContent = ToDecode;

        // & muﬂ als erstes!
        ReturnContent = ReturnContent.Replace( "&amp;", "&" );
        ReturnContent = ReturnContent.Replace( "&lt;", "<" );
        ReturnContent = ReturnContent.Replace( "&gt;", ">" );
        ReturnContent = ReturnContent.Replace( "&apos;", "\'" );
        ReturnContent = ReturnContent.Replace( "&quot;", "\"" );

        return ReturnContent;
      }


      public void AddAttribute( string attribute, string content )
      {
        /*
        if ( content.Length > 0 )
        {
          if ( ( content[0] == '"' )
          &&   ( content[content.Length - 1] == '"' ) )
          {
            content = content.Substring( 1, content.Length - 2 );
          }
        }
         */

        content = XMLEncode( content );

        if ( HasAttribute( attribute ) )
        {
          SetAttribute( attribute, content );
        }
        else
        {
          m_Attributes.Add( new XMLAttribute( attribute, content ) );
        }
      }



      public void SetAttribute( string attribute, string content )
      {
        content = XMLEncode( content );
        foreach ( XMLAttribute attrib in m_Attributes )
        {
          if ( attrib.Name == attribute )
          {
            attrib.Content = content;
            return;
          }
        }
        AddAttribute( attribute, content );
      }



      public bool HasAttribute( string attribute )
      {
        foreach ( XMLAttribute attrib in m_Attributes )
        {
          if ( attrib.Name == attribute )
          {
            return true;
          }
        }
        return false;
      }



      // adds new element(s)
      // several tags can be concattenated by dots
      //         in that case as many subelements as tags are created
      public XMLElement AddChild( string TagName, string Content )
      {
        string[]    parts = TagName.Split( '.' );

        XMLElement  currentParent = this;

        for ( int i = 0; i < parts.Length - 1; ++i )
        {
          XMLElement    existingChild = currentParent.FindByType( parts[i] );
          if ( existingChild != null )
          {
            currentParent = existingChild;
          }
          else
          {
            XMLElement    newChild = new XMLElement( parts[i] );
            currentParent.AddChild( newChild );
            currentParent = newChild;
          }
        }

        XMLElement    newElement = new XMLElement( parts[parts.Length - 1], Content );

        currentParent.AddChild( newElement );
        return newElement;
      }



      public void AddChild( XMLElement element )
      {
        m_Childs.Add( element );
        element.Parent = this;
      }



      public void RemoveChild( XMLElement element )
      {
        m_Childs.Remove( element );
        element.Parent = null;
      }



      public List<XMLElement> ChildElements
      {
        get
        {
          return m_Childs;
        }
      }




      // searches the direct childs for the given type
      // dots are treated as hierarchy, e.g. "Receiver.Account.IBAN" is
      // treated like this: Search childs for element "Receiver", if found
      // search Receivers childs for "Account" and so on

      public XMLElement FindByType( string type )
      {
        string[]    parts = type.Split( '.' );

        XMLElement  currentElement = this;

        foreach ( XMLElement element in currentElement.m_Childs )
        {
          if ( element.Type == parts[0] )
          {
            if ( parts.Length == 1 )
            {
              // last element reached
              return element;
            }
            return element.FindByType( string.Join( ".", parts, 1, parts.Length - 1 ) );
          }
        }
        return null;
      }



      public XMLElement FindByTypeRecursive( string type )
      {
        if ( Type == type )
        {
          return this;
        }
        foreach ( XMLElement element in m_Childs )
        {
          XMLElement subElement = element.FindByTypeRecursive( type );
          if ( subElement != null )
          {
            return subElement;
          }
        }
        return null;
      }



      public void SetContent( string content )
      {
        this.content = content;
      }



      public string ToText( int iIndent )
      {
        string indent = "";

        for ( int i = 0; i < iIndent; ++i )
        {
          indent += ' ';
        }

        string result = "";

        if ( Type == "" )
        {
          // a comment tag
          result += indent + '<' + Content + ">\r\n";
          return result;
        }

        result += indent;
        result += '<';
        result += Type;

        foreach ( XMLAttribute attrib in m_Attributes )
        {
          result += ' ' + attrib.Name + "=\"" + attrib.Content + "\"";
        }
        if ( m_Childs.Count > 0 )
        {
          result += ">\r\n";
        }
        else if ( ( m_Childs.Count == 0 )
        && ( Content.Length == 0 ) )
        {
          // keine Childs
          result += "/>\r\n";
          return result;
        }
        else
        {
          result += '>';
        }

        foreach ( XMLElement child in m_Childs )
        {
          result += child.ToText( iIndent + 2 );
        }
        if ( Content.Length > 0 )
        {
          if ( m_Childs.Count != 0 )
          {
            result += indent;
          }
          result += Content;
          if ( m_Childs.Count != 0 )
          {
            result += "\r\n";
          }
        }
        else if ( m_Childs.Count != 0 )
        {
          result += indent;
        }
        result += "</" + Type + ">\r\n";
        return result;
      }

      public void removeAttribute( string attribute )
      {
        foreach ( XMLAttribute attrib in m_Attributes )
        {
          if ( attrib.Name == attribute )
          {
            m_Attributes.Remove( attrib );
            return;
          }
        }
      }



      public string Attribute( string attribute )
      {
        foreach ( XMLAttribute attrib in m_Attributes )
        {
          if ( attrib.Name == attribute )
          {
            return XMLDecode( attrib.Content );
          }
        }
        return "";
      }



      public void Clear()
      {
        foreach ( XMLElement child in m_Childs )
        {
          child.Clear();
        }
        m_Childs.Clear();

        Content = "";
        m_Attributes.Clear();
      }



      public XMLElement FirstChild()
      {
        if ( m_Childs.Count == 0 )
        {
          return null;
        }
        return m_Childs[0];
      }



      public XMLElement GetNextSibling( XMLElement element )
      {
        int index = m_Childs.IndexOf( element );
        if ( ( index == -1 )
        ||   ( index + 1 >= m_Childs.Count ) )
        {
          return null;
        }
        return m_Childs[index + 1];
      }
    }

    public class XMLParser : XMLElement
    {

      private LinkedList<XMLElement> m_listOpenTags = new LinkedList<XMLElement>();
      private int                     m_iCurrentParsePos = 0;
      private string                  m_Error = "";
      
      public XMLParser()
      {
      }
      
      public string ParseError()
      {
        return m_Error;
      }
      
      public string ToText()
      {
        int   iIndent = 0;

        string result = "<?xml version=\"1.0\"?>\r\n";
        
        foreach ( XMLElement element in m_Childs )
        {
          result += element.ToText( iIndent );
        }
        return result;
      }
      
      private int ParseDTDTag( string xmlText, int nextContentPos )
      {
        int     iTagStart = xmlText.IndexOf( "<!", nextContentPos );
        if ( iTagStart == -1 )
        {
          return -1;
        }
        // einfach ¸berlesen
        int tagEnd = xmlText.IndexOf( '>', iTagStart );
        int bracketStart = xmlText.IndexOf( '[', iTagStart );
        if ( bracketStart == -1 )
        {
          // keine eckigen Klammern
          return tagEnd + 1;
        }
        else
        {
          if ( tagEnd < bracketStart )
          {
            // da ist direkt ein Ende
            return tagEnd + 1;
          }
          // verschachtelte DTD-Elemente
          int bracketEnd = xmlText.IndexOf( ']', bracketStart );
          if ( bracketEnd == -1 )
          {
            return -1;
          }
          int tagClose = xmlText.IndexOf( '>', bracketEnd );
          if ( tagClose == bracketEnd + 1 )
          {
            return tagClose + 1;
          }
        }
        return -1;
      }

      public bool Parse( System.IO.Stream ioIn )
      {
        if ( ( ioIn == null )
        ||   ( !ioIn.CanRead ) )
        {
          return false;
        }
        try
        {
          byte[] payload = new byte[ioIn.Length];
          ioIn.Read( payload, 0, (int)ioIn.Length );
          string   inBytes = System.Text.Encoding.Default.GetString( payload );
          ioIn.Close();
          return Parse( inBytes );
        }
        catch ( Exception )
        {
          return false;
        }
      }
      
      public bool Parse( string xmlText )
      {
        Clear();
        xmlText = xmlText.Trim();
        m_iCurrentParsePos = 0;
        m_listOpenTags.Clear();
        if ( xmlText.Length == 0 )
        {
          m_Error = "XML is empty";
          return false;
        }
        // Header zuerst
        int iTagStartPos = xmlText.IndexOf( '<' );
        if ( iTagStartPos != 0 )
        {
          m_Error = "No start tag found";
          return false;
        }
        int iTagEndPos = xmlText.IndexOf( '>' );
        if ( iTagEndPos == -1 )
        {
          m_Error = "No end tag found";
          return false;
        }
        string  headerTag = xmlText.Substring( iTagStartPos + 1, iTagEndPos - iTagStartPos - 1 );
        if ( headerTag.Length == 0 )
        {
          m_Error = "header tag is empty";
          return false;
        }
        if ( ( !headerTag.StartsWith( "?" ) )
        ||   ( !headerTag.EndsWith( "?" ) ) )
        {
          m_Error = "invalid header tag";
          return false;
        }
        
        /*
        // Neu - DTD-Elemente
        int     nextContentPos = iTagEndPos + 1;
        if ( xmlText.IndexOf( "<!" ) != -1 )
        {
          nextContentPos = ParseDTDTag( xmlText, nextContentPos );
          if ( nextContentPos == -1 )
          {
            m_Error = "Error parsing DTD Tags";
            return false;
          }
        }*/

        int     nextContentPos = iTagEndPos + 1;

        // jetzt die einzelnen Elemente
        m_listOpenTags.AddLast( this );
        if ( !ParseTag( xmlText, nextContentPos ) )
        {
          Clear();
          m_listOpenTags.Clear();
          return false;
        }
        m_listOpenTags.Clear();

        return true;

      }



      private bool ParseTag( string xmlText, int iCurrentPos )
      {
        while ( iCurrentPos < xmlText.Length )
        {
          string    strTagContent = "";
            
          m_iCurrentParsePos = iCurrentPos;
          // Tag-Anfang
          int iTagStartPos = xmlText.IndexOf( '<', iCurrentPos );
          if ( iTagStartPos == -1 )
          {
            //System.out.println( "no tag start<" );
            m_Error = "No tag start at " + iCurrentPos;
            return false;
          }
          if ( iTagStartPos > m_iCurrentParsePos )
          {
            strTagContent = xmlText.Substring( m_iCurrentParsePos, iTagStartPos - m_iCurrentParsePos );
            
            strTagContent = strTagContent.Trim();
          }
          int iTagEndPos = xmlText.IndexOf( '>', iTagStartPos );
          if ( iTagEndPos - iTagStartPos < 2 )
          {
            //System.out.println( "no tag end>" );
            m_Error = "No tag end at " + iTagStartPos;
            return false;
          }
          string        tagContent = xmlText.Substring( iTagStartPos + 1, iTagEndPos - iTagStartPos - 1 );
          string        tagCurrent = "";

          //System.out.println( "tagContent " + tagContent );

          XMLElement    currentTag = null;
          XMLElement    curElement = new XMLElement();
          
          if ( m_listOpenTags.Count > 0 )
          {
            currentTag = m_listOpenTags.Last.Value;
          }
          if ( currentTag != null )
          {
            currentTag.Content = strTagContent;
          }

          if ( tagContent.StartsWith( "!--" ) )
          {
            if ( !tagContent.EndsWith( "--" ) )
            {
              m_Error = "Comment tag not ending as expected";
              return false;
            }
            curElement.Content = tagContent;
            m_Childs.Add( curElement );
            m_iCurrentParsePos = iTagEndPos + 1;
            iCurrentPos = m_iCurrentParsePos;
            continue;
          }


          if ( tagContent.StartsWith( "/" ) )
          {
            // Ende-Tag
            if ( m_listOpenTags.Count == 0 )
            {
              //System.out.println( "Closing tag but no more open tags" );
              m_Error = "closing tag but no more open tags";
              return false;
            }
            //System.out.println( "closing tag: " + tagContent + " Open " + m_listOpenTags.size() );
            tagContent = tagContent.Substring( 1 );
            if ( tagContent != currentTag.Type )
            {
              //System.out.println( "closing tag mismatch " + tagContent + " != " + currentTag.getType() );
              m_Error = "closing tag mismatch " + tagContent + " != " + currentTag.Type;
              return false;
            }
            m_iCurrentParsePos = iTagEndPos + 1;
            return true;
          }

          bool   autoClosing = tagContent.EndsWith( "/" );
          if ( autoClosing )
          {
            tagContent = tagContent.Substring( 0, tagContent.Length - 1 );
          }
          
          int           iSpacePos = tagContent.IndexOf( ' ' );
          if ( iSpacePos == -1 )
          {
            // nur das Element selbst, keine Attribute
            tagCurrent = tagContent;
          }
          else
          {
            tagCurrent = tagContent.Substring( 0, iSpacePos );
          }

          if ( !autoClosing )
          {
            m_listOpenTags.AddLast( curElement );
            
            //System.out.println( "=Tag Open (" + tagContent + ") = " + m_listOpenTags.size() );
          }
          else
          {
            //tagContent = tagContent.substring( 0, tagCurrent.length() );
          }

          curElement.Type = tagCurrent;
          
          if ( currentTag != null )
          {
            //System.out.println( "Adding Tag " + tagCurrent + " as child of " + currentTag.getType() );
            currentTag.AddChild( curElement );
          }
          //addChild( curElement );
          if ( iSpacePos != -1 )
          {
            // Attribute parsen
            int     iAttrPos = iSpacePos + 1;

            while ( iAttrPos < tagContent.Length )
            {
              // Attribut-Ende suchen
              int   iEqualPos = tagContent.IndexOf( '=', iAttrPos );
              if ( iEqualPos == -1 )
              {
                  //System.out.println( "attribut missing =" );
                  m_Error = "attribut missing =";
                  return false;
              }
              // Attribut kann Leerzeichen enthalten!!
              string    attributName = tagContent.Substring( iAttrPos, iEqualPos - iAttrPos ).Trim();
              string    attribut = "";
              int   attributeEnd = -1;
              if ( tagContent[iEqualPos + 1] == '\"' )
              {
                int   secondApostrophe = tagContent.IndexOf( '\"', iEqualPos + 2 );
                if ( secondApostrophe == -1 )
                {
                  //System.out.println( "attribute missing second apostrophe" );
                  m_Error = "attribute missing second apostrophe";
                  return false;
                }
                attributeEnd = secondApostrophe + 1;
                attribut = tagContent.Substring( iEqualPos + 1, secondApostrophe + 1 - iEqualPos - 1 );
              }
              else
              {
                // Attribut-Ende beim n‰chsten Leerzeichen
                attributeEnd = tagContent.IndexOf( ' ', iAttrPos );
                attribut = tagContent.Substring( iEqualPos + 1, attributeEnd - iEqualPos - 1 );
              }

              if ( ( attribut.Length >= 2 )
              &&   ( attribut[0] == '"' )
              &&   ( attribut[attribut.Length - 1] == '"' ) )
              {
                attribut = attribut.Substring( 1, attribut.Length - 2 );
              }
              
              //System.out.println( "Checking attribute: " + attributName + " = " + attribut );
              curElement.AddAttribute( attributName,
                                       attribut );
              iAttrPos = attributeEnd + 1;
            }
          }

          if ( !ParseTag( xmlText, iTagEndPos + 1 ) )
          {
            //System.out.println( "Failed to parse child tag" );
            //m_Error = "Failed to parse child tag";
            return false;
          }
          
          if ( !autoClosing )
          {
            m_listOpenTags.RemoveLast();
            //System.out.println( "=Tag Close (" + tagContent + ") = " + m_listOpenTags.size() );
          }
          else
          {
            return true;
            //System.out.println( "==Not autoclosing tag (" + tagContent + ") = " + m_listOpenTags.size() );
          }

          iCurrentPos = m_iCurrentParsePos;
        }
        return true;
      }
      
    }

  };
};
