# TImeWiseBackEnd

# Use Cases

## USER Requests
Base_Url = https://{url}/User
### Register User
1.	Add user to firebase authentication via the email and password provided.
2.	Add user to database using api request <br>
Request Type: POST <br>
URL: Base_Url + “/AddUser” <br>
Body: <br> 
{ <br> 
	“name”: {}, <br> 
	“email”: {}, <br> 
	“job”: {}, <br>
	“password”: {}, <br> 
	“min”: {}, <br> 
	“max”: {} <br>
}
### Edit User
Edit user in database <br>
Request Type: POST <br>
URL: Base_Url + “/EditUser/?UserId={UserId}” <br>
Body: <br>
{ <br>
	“name”: {}, <br>
	“email”: {}, <br>
	“job”: {}, <br>
	“password”: {}, <br>
	“min”: {}, <br>
	“max”: {} <br>
}
BodyInformation: you can leave out any of these fields and the user will still be updated <br>

### Get User
Get User from database <br>
Request Type: GET <br>
URL: Base_Url + “/GetUser/?UserId={UserId}” <br>
### Get All Users
Get Users from database <br>
Request Type: GET <br>
URL: Base_Url + “/GetAllUsers”v
### Delete User
Delete user from database  <br>
Request Type: DELETE <br>
URL: Base_Url + “DeleteUser/?UserId={UserId}” <br>

## Category Requests
Base_Url = https://{url}/Category <br>
### Create Category
Add category to database using api request <br>
Request Type: POST <br>
URL: Base_Url + “/AddCategory” <br>
Body: <br>
{ <br>
  "userId": {}, <br>
 "name": {} <br>
} <br>
### Edit Category
Edit category in database <br>
Request Type: POST <br>
URL: Base_Url + “/EditCategory/?CategoryId={CategoryId}” <br>
Body: <br>
{
  "userId": {}, <br>
 "name": {} <br>
} <br>
BodyInformation: you can leave out any of these fields and the user will still be updated <br>

### Get Category
Get Category from database <br>
Request Type: GET <br>
URL: Base_Url + “/GetCategory/?CategoryId={CategoryId}” <br>
### Get All Categories
Get Categories from database <br>
Request Type: GET <br>
URL: Base_Url + “/GetAllCategories” <br>
### Delete Categories
Delete Categories from database  <br>
Request Type: DELETE <br>
URL: Base_Url + “DeleteCategories/?DeleteCategories={DeleteCategories}” <br>
### Get All User Categories
Get all categories from database for a specific user <br>
Request Type: GET <br>
URL: Base_Url + “GetAllUserCategories/?UserId={UserId}” <br>
### Get All User Categories With Hours Sum
Get all categories from database for a specific user with a totalHours field from timesheet <br>
Request Type: GET <br>
URL: Base_Url + “GetAllUserCategoriesWithHoursSum/?UserId={UserId}” <br>
### Get All User Categories With Hours Sum Within Date Range
Get all categories from database for a specific user with a totalHours field from timesheet and within a date range  <br>
End date can be left blank and it will get all categories from start to current date <br>
Request Type: GET <br>
URL: Base_Url + “GetAllUserCategoriesWithHoursSumWithinDateRange/?UserId={UserId}&start={startDate}&end={enddate}” <br>
### Get User Category With Hours Sum Within Date Range
Get category from database for a specific user with a totalHours field from timesheet and within a date range  <br>
End date can be left blank and it will get all categories from start to current date <br>
Request Type: GET <br>
URL: Base_Url + “GetUserCategorieWithHoursSumWithinDateRange/?UserId={UserId}&CategoryId={CategoryId}&start={startDate}&end={enddate}” <br>

## Picture Requests
Base_Url = https://{url}/Picture<br>
### Create Picture <br>
Add user to database using api request<br>
Request Type: POST <br>
URL: Base_Url + “/AddPicture”<br>
Body:<br>
{<br>
  "userId": {},<br>
  "description": {}<br>
}<br>
### Edit Picture
Edit picture in database<br>
Request Type: POST<br>
URL: Base_Url + “/EditPicture/?PictureId={PictureId}”<br>
Body:<br>
{<br>
  "userId": {},<br>
  "description": {}<br>
}<br>
BodyInformation: you can leave out any of these fields and the user will still be updated<br>
### Get Picture
Get picture from database<br>
Request Type: GET<br>
URL: Base_Url + “/GetPicture/?PictureId={PictureId}”<br>
### Get All Pictures
Get Pictures from database<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllPictures”<br>
### Delete Picture
Delete picture from database <br>
Request Type: DELETE<br>
URL: Base_Url + “DeletePicture/?PictureId={PictureId}”<br>

## Timesheet Requests
Base_Url = https://{url}/Timesheet<br>
### Create Timesheet
Add timesheet to database using api request<br>
Request Type: POST<br>
URL: Base_Url + “/AddTimesheet”<br>
Body:<br>
{<br>
  "categoryId": "string",<br>
  "pictureId": "string",<br>
  "description": "string",<br>
  "hours": 0<br>
}<br>
### Edit Timesheet
Edit timesheet in database<br>
Request Type: POST<br>
URL: Base_Url + “/EditTimesheet/?TimesheetId={TimesheetId}”<br>
Body:<br>
{<br>
  "categoryId": "string",<br>
  "pictureId": "string",<br>
  "description": "string",<br>
  "hours": 0<br>
}<br>
BodyInformation: you can leave out any of these fields and the user will still be updated<br>

### Get Timesheet
Get timesheet from database<br>
Request Type: GET<br>
URL: Base_Url + “/GetTimesheet/?TimesheetId={TimesheetId}”<br>
### Get All Timesheets
Get all timesheets from database<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheets”<br>
### Get All User Timesheets
Get Users timesheet from database<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllUserTimesheets?UserId={UserId}”<br>
### Get All Timesheets On Week
Get all timesheets from database for a specific week <br>
The date can be any date within the week you are wanting<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheetsOnWeeks?date={Date}”<br>
### Get All Timesheets On Months
Get all timesheets from database for a specific month <br>
The date can be any date within the month you are wanting<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheetsOnMonths?date={Date}”<br>
### Get All Timesheets within Range
Get all timesheets from database for a specific user in a specific date range<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheetsInRange?start={Date}&end={Date}&UserId={UserId}”<br>
### Get All Timesheets of users category
Get all timesheets from database for a specific users category<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheetsOfUserCategory?UserId={UserId}&CategoryId={CategoryId}”<br>
### Get All Timesheets within Range and Category<br>
Get all timesheets from database for a specific date range, Category and User<br>
Request Type: GET<br>
URL: Base_Url + “/GetAllTimesheetsInRangeAndCategory?start={Date}&end={Date}&UserId={UserId}&CategoryId={CategoryId}”<br>
### Delete Timesheet
Delete timesheet from database <br>
Request Type: DELETE<br>
URL: Base_Url + “DeleteTimesheet/?TimesheetId={TimesheetId}”<br>



