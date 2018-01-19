{"total":7,"rows":[
	{"name":"id","value":"Bill Smith","group":"元素","editor":"text"},
	{"name":"name","value":"","group":"元素","editor":"text"},
	{"name":"class","value":"40","group":"元素","editor":"numberbox"},
	{"name":"style","value":"01/02/2012","group":"元素","editor":"datebox"},
	{"name":"数据源","value":"123-456-7890","group":"数据","editor":"text"},
	{"name":"数据项","value":"bill@gmail.com","group":"数据","editor":{
		"type":"combobox",
		"options":{
            valueField: 'label',
		    textField: 'value',
		    data: [{
		    	label: 'java',
		    	value: 'Java'
		    },{
			     label: 'perl',
			     value: 'Perl'
		    },{
		    	label: 'ruby',
		    	value: 'Ruby'
		    }] 		
	     }
	}},
	{"name":"FrequentBuyer","value":"false","group":"Marketing Settings","editor":{
		"type":"checkbox",
		"options":{
			"on":true,
			"off":false
		}
	}}
]}