    	//ds="pt_productModel.data"
      function GetTreeID(ds,parentID)
    	{
        var id;
    		
        var data=$.rCall(ds, { page:1,rows:1000,sort:"ID",order:"asc" ,filterRules:[{field:'ParentId',op:'equal',value:parentID }] });
    		if(data.total!=0)
    			id=parseInt(data.rows[data.total-1].ID)+1;
    		else
    			id=parentID+"01";

    		return id;  		
    	}

