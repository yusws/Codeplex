# WSS Activities
The original project is located here: https://wssactivities.codeplex.com/


##Project Description
Contains 17 new SharePoint Designer workflow activities, mostly revolving around the site provisioning process, though some allow you to copy documents and list items to another site collection.

##Setup

###Site Management Activities
These allow you to manipulate various aspects of a site as well as create new sites. I created a site provisioning workflow for a client and this was every activity I needed that wasn’t provided out of the box. Documentation for each of these, though the activities’ names are probably enough to get you started. 

- Create Site Collection 
- Create SubSite 
- Lookup Site Template ID 
- Set Site Title 
- Set Site Theme 
- Create Site Group 
- Setup Site Group 
- Set Available Templates 
- Activate a Feature 
- Set Site Masterpage 
- Set Portal Link 
- Set Site Property 
- Get Site Property


###Item Management Activities
These activities allow you to manipulate specific items or documents within a site collection or across the site collection boundary. Of these, Copy, Delete, and Update are fairly straight forward. It’s “Publish Item and Link to Another Location” that requires some further explanation. This activity creates a remote copy of a source item and puts in place an event handler that keeps the remote copy up to date with the source version. Additional columns are added to the source list and destination list to track the linkage between the two. 

- Publish Item and Link to Another Location 
- Copy Item To Another Location 
- Delete Remote Item 
- Update Remote Copy