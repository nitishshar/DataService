# DataService
 
Sample Request

{
	"StartRow": 101,
	"EndRow": 1000,
	"RowGroupCols": [
		{
			"Id": "1",
			"DisplayName": "Employee Number",
			"Field": "emp_no",
			"AggFunc": ""
		},
		{
			"Id": "2",
			"DisplayName": "First Name",
			"Field": "first_name",
			"AggFunc": ""
		},
		{
			"Id": "2",
			"DisplayName": "last name",
			"Field": "last_name",
			"AggFunc": ""
		}
	],
	"ValueCols": [
		{
			"Id": "3",
			"DisplayName": "Salary",
			"Field": "salary",
			"AggFunc": "sum"
		}
	],
	"PivotCols": [
		{
			"Id": "4",
			"DisplayName": "Title",
			"Field": "title",
			"AggFunc": null
		}
	],
	"PivotMode": true,
	"GroupKeys": [],
	"FilterModel": {
		"title": {
			"Values": [
				"Staff",
				"Senior Engineer"
			],
			"FilterType": "set",
            "$type":"SetColumnFilter"
		}
	},
	"SortModel": []
}