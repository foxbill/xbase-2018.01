 function HtmlEncode (theString) {
      // body...
       theString = theString.replace(/>/g, "&gt;");
            theString = theString.replace(/</g, "&lt;");
            //theString = theString.replace(" ", "&nbsp;");
           // theString = theString.replace(" ", "&nbsp;");
            theString = theString.replace(/\"/g, "&quot;");
            theString = theString.replace(/\'/g, "'");
          //  theString = theString.replace("\n", "<br/> ");
            return theString;
    }

     function HtmlDiscode (theString) {
      // body...
                   theString = theString.replace(/&gt;/g, ">");
            theString = theString.replace(/&lt;/g, "<");
          //  theString = theString.replace("&nbsp;", " ");
         //   theString = theString.replace("&nbsp;", " ");
            theString = theString.replace(/&quot;/g, "\"");
            theString = theString.replace(/'/g, "\'");
          //  theString = theString.replace("<br/> ", "\n");
            return theString;
    }