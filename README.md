<h2> Visual Behaviour Tree for Unity</h2>

<p> Behaviour tree is a package for Unity that contains that ability to program Unity AI in an efficient way. </p>
<p> The goal was to create a node based behavior tree system for AI in Unity to improve efficiency for achieving developing complex AI behaviors. </p>
<p> Inspired my Unreal Engine's Behavior Tree Design and needed an editor to create AI Bahviours as quickly as possible this Behavior Tree Was created.
<p> Uses Scriptable objects to store the behaviour tree and blackboard data <p>

![Editor](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/BehaviourTreeEditor.png?raw=true)
![Runtime](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/PlayMode.png?raw=true)

<h2> Features </h2>
<h3> Undo/Redo <h3>

<h3> BlackBoard </h3>

<p> The Blackboard is an asset that can be created from the assets create menu.
  right-click->Behavior Tree->Blackboard.
  Just like Unreal blackboard asset, different keys can be added to store certain data that a behavior tree can access.
  Key Types include:
  <ul>
    <li>Bool</li>
    <li>Int</li>
    <li>Float</li>
    <li>Enum</li>
    <li>String</li>
    <li>Vector3</li>
    <li>Unity's Gameobject</li>
    <li>Unity's Object</li>
    </ul>
Each key constists of the following :
  <ul>
  <li>Description</li>
  <li>Key Name</li>
  <li>Instance - Which determines if the key is the same across different trees</li>
  </ul>
  </p>
  
![BlackBoard](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/BlackBoardEditor.png?raw=true)

<h3> Nodes Currently Available </h3>

![Nodes](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/NodeTypes.png?raw=true)

<h4>Composites</h4>
 <ul>
  <li>Selector - Runs first successful child</li> 
  <li>Seqeunce - Runs though all children until a failure is returned</li>
  <li>Fallback - Runs until one its children returns success</li> 
 </ul>
 
 <h4>Decorators</h4>
 <ul>
  <li>Black Board Based Condition</li> 
  <li>Compare Black Board Entries</li>
  <li>Conditional Loop - Repeats based on a condition</li> 
  <li>Force Success</li> 
  <li>Repeat - Finite or Infinite Loop</li> 
 </ul>
 
 <h4>Tasks</h4>
 <ul>
  <li>Debug</li> 
  <li>Generate Random Location</li>
  <li>Move To Location</li>
  <li>Wait</li>   
 </ul>
 
  <h4>Utility AI Nodes</h4>
 <ul>
  <li>Utility Decision</li>  
  <li>Utility Action - has a second pin to execute a subgraph as an option to execute an action instead of deriving from this node</li> 
  <li>Utility Consideration</li> 
 </ul>
 
 <h4>Miscellaneous</h4>
 <ul>
  <li>Group - Groups Nodes Together</li>  
 </ul>

<h3> Settings </h3>

![Settings](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/Settings.png?raw=true)

<p>
	The settings of the behavior tree can be set in the project Settings.
	The user can change the node style to a custom uxml element
	The templates for creating a new decorator or task script can also be set, as well the icon used for 
	blackboard keys
</p>

<h3> Menu Options </h3>

![Options](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/ToolBar.png?raw=true)

<p>
	When opening a behavior tree there are menu options that can found.
	<ul>
		<li> Current Scene AI - When the editor is opened and the editor is played a list will generate
		with all the ai using this Behviour Tree -selecting one will show the nodes that are running
		</li>
		<li> New BlackBoard - allows creation of a new BlackBoard asset </li>
		<li> New Decorator - allows creation of a new Decorator Script </li>
		<li> New Task - allows creation of a new Task Script </li>
		<li> MiniMap -Togglesthe minimap </li>
		<li> Save - Save the behavior tree directly </li>
	</ul>

<p>

<h3> Debugging </h3>
<p> Selecting an AI using a behavior tree, the scene editor will displaty info about the behaviour tree as well as the current data of each blackboard keys.
	A breakpoint can be added to a node in order to pause the behavior tree. </p>

![Debug](https://github.com/Darius000/BehaviourTree/blob/master/ScreenShots/Debug.png?raw=true)

<h4>Note:</h4>
<p>The base project is based off the Unity's shader graph but doesn't
require it as a dependency. </p>
