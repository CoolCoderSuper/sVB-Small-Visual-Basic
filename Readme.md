# Contents
- Small Visual Basic (sVB):
- Using the source code:
- Download the language:
- Try the samples:
- Why do we need sVB:
- Form designer Features:
- SB Code Enhancements:
- Create a code library!

# What's new in sVB v2.5:
sVB is ready to create productive desktop apps.
1. The toolbox now contains four new controls:
* ProgressBar, ScrollBar and Slider: These bars are horizontal only, but you can use the rotate thumb of the control on the designer to rotate them to become vertical! For more info, see the `Progress` and `Slider Color Composer` apps in the samples folder.
* ToggleButton: This control can be checked or unchecked but it seems like a button that appears clicked or un-clicked. See the `Toggle Buttons` app in the samples folder.

2. You can add many timers to the form, by calling the `Form.AddTimer` Method. Small Basic library already contains the Timer object, but it is only one timer, and it makes code harder to make it handle different actions in different intervals. Besides, it may not work properly with forms and controls, and of course it will be missy if you try to use this single timer to handle different actions in many forms. So, to make things easier, you can now add as many timers as you need to each form!
For example, the `Stop Watch` app in the samples folder uses two timers, one to update the current date and time displayed on the form title bar, and the other one to count the elapsed time for the stop watch.

3. You can show the open file and save file dialogs to the user by calling `File.OpenFileDialog` and `File.SaveFileDialog` methods. You must send the extensions filter to these methods, which controls the types of files that the browser will show to the user. You can use the standard .NET filter string like `"Text Files|*.txt;*.rtf|Docs|*.doc"`, or you can send the filter as an array like 
```vb
fileName = File.OpenFileDialog ({
   {"Text Files", "txt", "rtf"},
   {"Docs", "doc"}
})
```

For an example, see how these methods are used to open and save files in the `sVB notepad` app in the samples folder.

4. You can show the font dialog to let the user choose font properties, by calling the ` Desktop.ShowFontDialog` method. This method empty string "" if the user canceled the operation, otherwise it returns an array containing the font properties under the keys Name, Size, Bold, Italic, Underlined and Color, so you can use these keys as dynamic properties:
```vb
Font = Desktop.ShowFontDialog(Me.Font)
Me.FontName = Font!Name
Me.FontSize = Font!Size
```

In the above example, you see that the ShowFontDialog has a parameter that receives an array containing the initial font properties. To make this easy, each control now has a Font property that returns an array with the font properties. You can also use this property to change font properties of the control in one step:
```vb
font = Desktop.ShowFontDialog(Me.Font)
If font <>"" Then
   Me.Font = font
EndIf
```

And to make this even easier, each control has a `ChooseFont` methods that shows the font dialog and sets its result to the controls Font property, so the above 4 lines can be replaced with this single line (or in fact these two words!):
```
Me.ChooseFont()
```

5. If you want to crate a custom font dialog, you will need to know the font names defined on the user's system. In such case, use the `Desktop.FontNames` property to get an array containing these font names.
For a sample on how to create a custom font dialog, see the `FrmOps` in the `sVB notepad` app in the samples folder.

6. You can show the color dialog to allow the user to choose a color, by calling the `Color.ShowDialog` method. And for simplicity, each control has the `ChooseForeColor` and `ChooseBackColor` methods to show the dialog and change the control fore and back colors directly.

7. You can now crate a main menu for the form by using the `Form.AddMainMenu` method, that returns the main menu object, which you can use it's `AddItem` method to add menu items.
For more info, see how the menus are defined in the `formMain` in the `sVB notepad` in the samples folder.

8. You can show a child form that it owened by a parent form by calling the `Form.ShowChildForm` method. 
A child form will alowys appear on top of its parent form even when you activate the parent form. If you hide or minimize the parent form, its child will be hideen with it, and when you show the parent form again, its child will appear again.
For example, this is how the FormMain shows the frmFind as a child form in the `sVB notepad` in the samples folder"
```
Me.ShowChildForm("FrmFind", TxtEditor)
```

Ther first argument is the name of the child form (and the auto completion list will offer the names of the forms for you), and the second param is an extra data that will be passed to the ArgsArr property in the child form.

9. You can change the style of the form and controls by loading styles from a recource dictionary. This is an advanced topic, that needs knowlage about XAML and WPF, but it always you to make use of syles and thems defiend for WPF, UWP or WinUI3 to make a beautiful design, and change how controls look and even work!
All you need is to have a Resource dictonary in a Xaml file, then use these two methods to load it:
* Control.SetRecourceDictionary: send the xaml file path to this method to load stryles from it into the control and its child controls. This means that if you called it from a form. the styles will affect all target controls if exists on it. Note that this method needs that styles have no keys. Styles with names (keys) will not be aplied to controls here.
*  Control. SetStyle: it is similal to the previous method, but it has a second parameter that recives the name (key) of the style. It is useful when you want to apply a style on only one control.
Note that if the style have no name, you can call SetRecourceDictionary from this control instead to apply the style on it, but this can also apply another styles on the controls if the resource dictionary has many styles targetting the same control type.
For mor info, see the `Custom Styles` app in the samples folder. the tow files `RoundCorner.style` and `RoundCorner2.style` are in fact XAML files, but I changed there extensions from `.xaml` to `.style` not to be cinfused with form design files. But if you used .xaml extension it will also work.

10. If you want something easier, you can change the Button and ToggleButton appearenace by setting the Flat property to True. This will help you use theem as a toolbar buttons by butting a label under them, as you can see in the ` Toggle Buttons` app in the samples folder.

Now we can announce sVB as a mature productive desktop programming language, and you can use it to build interesting apps. Have fun.

# Small Visual Basic (sVB):
sVB is an evolved version of Microsoft Small Basic with a small WinForms library and a graphics form designer. 

![Untitled](https://user-images.githubusercontent.com/48354902/126494834-f6c90190-4241-40c7-84b3-c3b3c432a6d1.png)

sVB has many enhancements over SB to make writing apps fast and easy  with little code. It brings back the joy and excitement of using vb6 to write RAD applications, by adding the illusion of the Object type while accessing controls members:
```vb
Label1.Text = TextBox1.Text + TextBox2.Text
```

All Label1, TextBox1 and TextBox2 are just string variables, but in sVB you can use the names of the controls as if they are Objects containing the controls themselves, and hence access their properties, methods and events directly, with support of the auto-completion list.

![untitled2](https://user-images.githubusercontent.com/48354902/126494901-60dfa36b-cdaf-4fd0-8107-1769f4e5c4c4.jpg)

You can also add event handlers from the upper dropdown lists: 
Choose the control name from the left list (say `Button1`), and click the event name from the right list (say `OnClick`), and this sub will be added for you in the code editor:
```vb
Sub Button1_OnClick
   
EndSub
```

Or you can simply double-click the button on the form designer, and the Button1_OnClick will be created for you!

To make this work, each form created by the designer has 3 files:
1. a `.xaml` file containing the form design.
2. a `.sb.gen` file, containing normal SB code that defines vars to hold form and controls names, and adds the event handlers instructions to connect each event with the sub that handles it.
3. a `.sb` file that you write your code in without warring about the other 2 files contents, as they are fully generated by the designer. This makes you write short, simple and clean code, focusing only on the task in hand. See the [samples folder](https://github.com/VBAndCs/sVB-Small-Visual-Basic/tree/master/Samples).

# Using the source code:
sVB is fully written with VB.NET, and targets .NET framework 4.8. You can run the source code in VS.NET 2019 and later.
before running the code, please copy the two folders `Lib` and `Toolbar` from the `SmallBasicIDE\SB.Lib` folder to the `SmallBasicIDE\bin\Debug` folder, as obviously `Git` execluds this folder, and I prefer it this way.

# Download the language:

Go to the [Releases page](https://github.com/VBAndCs/sVB-Small-Visual-Basic/releases), navigate to the latest version of vSB, expand the Assets list at the bottom of the page, and download the ZIP file.
Follow these instructions:
1. sVB needs .NET framework 4.8. If you don't have it on your PC, download and install it:
https://go.microsoft.com/fwlink/?LinkId=2085155

2. Unzip the `sVB.zip` file. You will have a folder with the same name where you unzipped the file. Open the folder and double-click `sVB.exe`.
And that it. You are ready to go :)

# Try the samples:
1. Right-click the form designer and click `Open` from the context menu. 
2. In the `open file dialog`, Navigate to the `sVB\Samples` folder and open any sample folder. 
3. Select the `.xaml` file from the folder and click the `Open` button. This will open the form in the designer.
4. Click the `Form code` tab at the top of the window to switch to the sb code editor.
5. Click the `Run` button (or hit F5 from keyboard) to run the program.

You can also open the sample folder in Windows Explorer, right-click the `.sb` and chose `open with` from the context menu, and choose sVB.exe as the default program to open `.sb` files. After that you can just double-click any `.sb` file to open it in sVB.

# Why do we need sVB:
BASIC is famous of being easy to learn, because its syntax is simple  and close to natural English. 
In 2008, MS released Small Basic for kids of 7 years old and above. It was really small, containing only 14 keywords to perform the basic programming instructions like `Sub`, `If`, `For`, `While` and `Goto` statements.

Small basic is a dynamic language, as it doesn't declare variables with types. You just assign a value to a valid identifier and SB will declare it as a Primitive variable, which can hold a string, a number, or an array.
This makes the language very easy to learn and use for kids.

But, a programming language is not just instructions. It has to have a class library to communicate with Windows. In fact SB comes with a number of powerful libraries, and allow you to supply your own libraries as well. This is where I saw a big issue while trying to teach SB to my nephews:
The language is too easy,  but using the libraries isn't that easy, especially when dealing with graphics and UI. The PDF book that comes with SB makes it hard, focusing on drawing shapes by using geometric functions (it even contains a fractals sample!)  
This is not the best way to introduce programming to kids. A black command window (the TextWindow) is easy but boring, while using vector graphics or drawing using the Turtle on the Graphics Window is amazing but can be quite hard.

The good news is that the Controls class allows you to draw a TextBox, and a Button on the GraphicsWindow, deal with their properties and handle their events. But, unfortunately, the kid has to design the form blindly while adding controls by code, and even worst,  the code used to communicate with these controls is verbose, because SB doesn't have the Object type as I explained above, so, you can only store the name of the control in a variable:
```vb
btn = Controls.AddButton("Enable", 100, 100)
Controls.ButtonClicked = OnClick
```

then send this variable to each method you use to alter the control:
```VB
Sub OnClick
   If Controls.GetButtonCaption(btn) = "Enable" Then
      Controls.SetButtonCaption(btn, "Disable")
   Else
      Controls.SetButtonCaption(btn, "Enable")
   EndIf
EndSub
```

This is not the kind of code you want to show to a kid!
In fact it will be easier to teach Visual Basic to the kid, so he can drag a button form the toolbox, drop it on the window, set it's name and caption from the properties window, double-click it to go to it's click event handler in the code editor, and just write:
```vb
   If btn.Text = "Enable"
      btn.Text = "Disable"
   Else
      btn.Text = "Enable"
   EndIf
```

And that's it. Fast, clean, easy and  short code, that made us love programming!

It is unbelievable that SB complicated such an easy task, in the name of being simple and easy to learn for kids!

I looked at some SB alternative IDEs but they are either:
- more complex (too advanced to do nothing important with a language meant to be a leering toy),
- or simple enough to draw the controls and generate some code for them, but still can't overcome the SB syntax limitations when dealing with objects.

This is when I decided to do something, and here we are.

# Form designer Features:
I will write this later, but the form designer is too easy to use, so, enjoy trying it.

# SB Code Enhancements:
I made many improvements to the SB compiler:
1. Support array initializers:

You can use the `{}` to set multiple elements to the array at once:
```vb
x = {1, 2, 3}
```

Nested initializers are also supported when you deal with multi-dimensional arrays:
```vb
y = {"a", "b", {1, 2, 3}}
```

You can also use vars inside the initializer, so, the above code can be rewritten as:
```vb 
y = {"a", "b", x}
```

And you can send an initializer as a param to a function:
```vb
TextWindow.WriteLine({"Adam", 11, "Succeeded"})
```

2. `For Next` and `While Wend`:

SB uses `EndFor` and `EndWhile` to close `For` and `While` blocks respectively. This is still supported in sVB but I allowed also to use `Next` to close `For` and `Wend` to close `While`, as they are used in VB6. I encourage you to use Next and Wend, as they give the meaning of repeating and circulating over the loop. `End` gives the meaning of finishing and exiting, so, it is suitable in `EndIf` and `EndSub`, but confusing in `EndFor` and `EndWhile`, as they can imply that `the loop finishes here`, not just `the block ends here`!

3. You can use `ExitLoop` to exit For and While loops, and `ContinueLoop` to skip the current iteration and jump back to the beginning of the loop body to continue the next iteration in for loop. Be aware that unlike For, while doesn't have an auto-incremented counter, so be sure you write the suitable code to update the variable that while condition depends on before using ContinueLoop inside the while block, otherwise you may end up stuck with an infinite loop.
In the case of nested 2 loops of any kind, you can exit the outer loop by using `ExitLoop -`. You can add more `-` signs to exit up levels loops in case you have 3 or 4 nested loops, or just use `ExitLoop *` to exit all nested loops at once. The same rules applies to `ContinueLoop` if you want to use it to cointinue outer loops.

4. You can use `Me` to refer to the current Form.

5. 'True' and 'False' are keywords of sVB.

6. Subroutines can have parameters now:
```vb
Sub Print(Name, Value)
   TextWindow.WriteLine("Name=" + Name + ", Value=" + Value)
EndSub
```

And call this sub like this:
```vb
  Print("Distance", 120)
```

Note that you can use `Return` inside the sub body to exit the sub immediately.


7. sVB can define functions now. You can supply params to get the fyunction input and use `Return` to return the function output.
```vb
Function Sum(x, y)
    Return x + Y
EndFunction
```

And you can use it like this:
```vb
x = Sum(1, 2)
```

8. SB doesn't have variable scope, as all variables are considered global, and you can define them in any place in the file and use them from any other place in the file (up or down). sVB has cleaned this mess, which is a break change that can prevent some SB code from running probably in sVB, but it is a necessary step to make the kid organize his code and write clean code. This is also necessary to make sub and function params work correctly, and allow you to use recursive subs and functions. The new scope rules are:
- Sub and function params are local, and hide any global vars with the same names.
- The For loop counter(iterator) in subs and functiins is local and hides any global var with the same name.
- Any var defined inside the sub or the function is local unless there is a global var with the same name is defined above of the sub function. If the global var is defined below, then the local var will hide it.

So, as a good practice:
- Define all global vars at the very top pf the file.
- Give global vars a prefix (such as `g_`) to avoid any conflictions with local vars.
- Don't use global vars unless there is no other solution, instead pass values to subs and functions through params, and receive values from functions through their return values.

9. The editor auto completes If, For, While, and Sub blocks just after writing a space after them.

10. The editor has a perfect auto-indentation.

11. Using naming convention to give sVB some info about the var type to make using them easier. 
- The first naming convention: Any var ends with or starts with a control name (like Form1 or myLabel) will be treated as a Label, so you can use the short syntax Control.Property and the auto completion list will appear to help you complete method and properties names.

- The second naming convention: Any var ends with or starts with the word Data is treated as a dynamic object, and you can add properties to it, and get auto completion support when you use them.
```
CarData.Color = Color.Red
CarData.Speed = 100
x = CarData.Speed
```

In fact, sVB converts the above syntax to:
```
CarData["Color"] = Color.Red
CarData["Speed"] = 100
x = CarData["Speed"]
```

Note that this naming convention rule ignores var domain rules, to allow you reuse the properties across subs and functions. This is totally safe as it only affects the property list that will appear in the auto completion list, but it has no effect on the variable domain rules when you run the program. You may see properties names from a data object from another function, but you still can't read these properties values in code. It is just a way to make coding faster and easier.
- The third naming convention: Any Data var that contains the name of another data var (after trimming the `Data` part of them bath) will show the union of their properties in the auto completion list. This allows you to `inherit` other data properties. As an example, of you use the names Car2Data, or myCarData in the above example, they will show the Color and Speed properties (coming from CarData) in the completion list"
```
Car2Data.Speed = 200
Car2Data.Acceleration = 10
```

And if you use `MyCar2Data` you will inherit all properties from `MyCarData`, `Car2Data` and `CarData`!

12. Use the vb lookup operator to crate dynamic properties:

```VB
Student!ID = 1
Student!Name = "Adam"
```

This is a shorter alternative to using the Data naming convention (see more details at the end of the file):
```VB
StudentData.ID = 1
StudentData.Name = "Adam"
```

13. Many enhancements in the WinForms controls and bug fixes.

14. You can split the code line over multi-lines by using the _ symbol at the end of each  
line segment. ex:
```VB..NET
Dim x = "First Line" + _
            "Second Line"
```

You can use _ at any position expet before of after the dot `.`.

15. You can also split the line at some positions without using the _ . These positions are:
- after the following symbols: `,`, `=`, `+`, `-`, `*`. `/`, `(`, `[`, `{`, `or`, `and`.
- before the following symbols: `+`, `-`, `*`. `/`, `)`, `]`, `}`, `or`, `and`.

Ex:
```VB
If x = y or 
    Text.GetSubText(
       "some text",
       6,
       3
   ) = "abc" Then

   x = 0
End If
```

16. You can add comments at the end of any line segment. Ex:
```VB
Function Add(  ' Adds two numbers
   a, _   ' first number
   b      ' second number
)
   Return a + b 
EndFunction
```

17. Adding an `(Add new function)` command to the upper right dropdown list in the code  
editor

18. Enhancing the auto completion list.

19. Enhancing the auto formatting of code, to adjust indentation of sub lines, and adjust  
spaces between tokens.

20. The code editor now highlights any matching pairs such as `()`, `[]` and `{}`. It also  
highlights the keywords of the Sub, Function, If, For, and While bolcks whenever you insert  
the caret on on of them. You can move to the next highlighted token by pressing `F4` or  
`Ctrl+Shift+Down`, and you can move to the previous highlighted token by pressing `Shift 
+F4` or `Ctrl+Shift+Up`.
You can also press such shortcuts keys on any line even there is no highlighted keywords.  
This will highlight the nearest block that contains the statement, and move to the neareest  
keyword up or down according to the shourtcut keys you are using.

21. Make it easy to crate a new form and show it from code:
```vb
form2 = Forms.AddForm("form2")
form2.Text = "Form2"
newButton = form2.AddButton(
      "Button 1"
      100,  ' left
      100,  ' Top
      400,  ' Width
      250   ' Height
)
newButton.Text = "Hello"
newButton.OnClick = Button1_OnClick
form2.Show()
```

This code can't run before sVB 1.6.5, because all controls were assumed to belong to Form1!. Now they belong to the forms that creates them using AddXXX methods (like AddButton, AddListBox... etc).
You can see an interesting example of this in the `Random Buttons` app in the Samples folder.

22. Add `OnShown` event to the Form. It is fired after the content of the form is rendered. If you use it, you should add all initialization into it, as you can't know for sure if it will occur before or after the code in the global section is executed! But you can be sure that all the controls are rendered and ready for use.
`OnShown` is the default event for the Form, and you can add a handler to it by just double-clicking the form surface in the form designer.

23. I got rid of the side help panel to save space, and showed the help info in a tip window that pops up after 2 seconds from moving the caret to any word in the code editor, and stays open for 10 seconds unless you move to another position, move the scroll, or press Esc key. I prevent showing the help for the same word unless toy move to another one, but you can force to show the help by pressing `F1`.
You can magnify the font of the popup help by pressing Ctrl and moving the mouse wheel. This is one of many functionalites built-in the WPF FlowDocument control that is used to show the help.
You can say I brought the VS intellisense to sVB. The pop up help offers valuable info about the current code token, including:
 * The scope (local or global var).
 * The definition signature (Type, Property, Dynamic Property, Event, Method Parameters).
 * If the token is a `variable`, a `sub` or a `function`, it will be shown as a link, so, you can click it to go to its definition line. If the token it the name of the form or a control on it, clicking the link will select the form or the control on the form designer.
* The documentation includes a summery, and info about parameters and return value. You can add a summery for user defined types by adding one or more comment lines above the var, sub, or function definitions. You can also add one more comment line at the end of the definition line. For subs and functions, you can add the additional summery line after the opining parans if you split the params over multi lines which also allows you to add a comment for each parameter to be used as a documentation. For Functions, the comment placed after the closing parans will be used as the documentation info for the return value. For subs, it will be considered an additional line of the summery. Ex:
```VB
XPos = 1   ' the horizontal position 

' adds x to the pos
Sub Move(
    x ' The increment value to add to the pos
)
   XPos = XPos + x
EndSub

Function InRange( ' Checks if the pos is withing the givin range
    start, ' the start position
    end  ' the end position
) ' True if the pos is in range, False otherwise.
   Return XPos >= start And XPos <= end
EndFunction
```

![image](https://user-images.githubusercontent.com/48354902/187048217-0f626439-8e90-4e90-b838-41cd3312ae4b.png)

While typing the arguments of the method call, the popup help will highlight the current param with a red color, and show only the info about this param, so you can focus only on the task in hand.

![image](https://user-images.githubusercontent.com/48354902/187048339-8861202b-9b86-41ce-9af9-b09c4dc3d7d1.png)

24. The editor formats the current sub after leaving a line that has changes. Formatting doesn't only include indentation, but also pretty listing of space between tokens, and fixing the casing of keywords, labels, type and method names. It also enforces using lower-case initial letters for local variables, and upper-case initial letters for global variables, labels, subs and functions.

25. The editor highlights every occurrence of the current identifier (variable, sub, function, label, type, method) name. Similar to highlighted block keywords, you can navigate between highlighted identifiers by pressing `F4` or `Ctrl+Shift+Up` or `Ctrl+Shift+down`

26. Many enhancements to the completion list to make it smarter, such as :
 * Filtering completion names by partial words (for ex: typing name can select MyName) 
 * Filtered out names don't appear in the list anymore.
 * The list remembers last selected object for each first letter.
 * The list remembers last selected method for each object.

27. `ForEach` statement is added to the language. It is easier for iterating through arrays that have items with string keys. Ex:
```vb
   arr[1] = "One" 
   arr["test"] = "Hello"
   arr!Name = "Ahmad"  
   ForEach item In arr
      TextWindow.WriteLine(item)
   Next
```

The above code will print:
```
One
Hello
Ahmad
```

28. Add `Append` and `AppendLine` methods to the TextBox control, and `Items` and `RemoveAllItems` to the ListBox control. See the [For Each](https://github.com/VBAndCs/sVB-Small-Visual-Basic/tree/master/Samples/For%20Each) Sample in the Samples folder.

29. Infer var types from initial values. This allows to call some methods directly from the var name. For example:
```VB
x = "abc"
x = x.Append("efg")
TextWindow.WriteLine(x) ' abcefg
```

Note That:
 * string variables call methods of the `Text` class.
 * double (numertic) variables call methods of the `Math` class.
 * color variables call methods of the `Color` class.
 * array variables call methods of the `Array` class.

The editor intellisense provides you with info about the var type and auto completion list shows the available methods it can call.
Note that:
 * sVB is still a dynamic type language, so, you can still store any value in the variable regardless its inferred type. I don't advice you to do that, as you should keep your code clean and readable.
 * sVB can't infer the type in some cases, such as:
- you initialize the var from a call to a function you wrote.
- you initialize the var from a calculated expression or operator, even a simple addition one, as sVB will decide the value at runtime only.
- sub and function params can't be inferred unless you named them using one of the naming conventions for data, controls, colors and keys.

In such cases, you can initialize the var with `""` for strings, `0` for numerics, or `{}` for arrays, then add the value to it in the next line.

30. Variables that contain the word `color` is considered to be a `Color` and when you assign a value for them, the auto completion list suggests the `Colors` class to choose a color from it's members.
The same for the word key, which is considered to be a `Key`, and auto completion offers the Keys class to choose from its members. This makes it easy to deal with the pressed key in Keyboard events, such as using the `Event.LastKey` property.

# What's new in sVB v1.8
31. sVB made some breaking changes to fix some SB issues: 
 a) this  funny code compiles in SB:
```VB
x = y
y = 1
```

where y is considered declared because it is assigned in the second line, and it's value will be "" in the first line! This will not compile anymore in sVB :)

 b) For loop final and step values can be changes in loop body in SB. For example, this loop is infinite in SB:
```VB
n = 5
For i = 1 to n
    TextWindow.RightLine(i)
    n = n + 1
EndIf
```
But now in sVB, final and step values are immune and can't be changed in loop body, so, the above for loop will print numbers from 1 to 5 and end normally, ignoring changes in `n` in loop body. This makes sVB consistent with VB6 and VB.NET, and it is also a good optimization, to avoid recalculating final and step expressions in every loop iteration.

 c) Now, you can write a loop like this:
```VB
For i = "a" to "z"
    TextBox1.AppendLine(i)
Next
```

This will show the ASCII codes of letters from a to z.

32. More enhancements in inferred variable types. For example, the For loop iterator\counter is now inferred as `Double`, and variables that are assigned to `Form.AddXX` methods are inferred as the type of the created control. In fact this was done already in previous version, but there was no auto-completion support for the variable unless it uses control naming convention. This is not necessary anymore, and auto-completion is supported for any name of the variable.

33. Introducing basic types naming conventions:
 * `str` for string variables.
 * `dbl` for double variables.
 * `date` for date variables.
 * `arr` for array variables.
These abbreviations can be use as prefixes or suffixes, but they should be distinguished from the rest of the word, by using _ (like `str_name`) or an uppercase letter for the next word (like `StrName`) or followed by a number (like `str1`), or if use them as a suffix, they should start with an upper case (like "myStr"). These rules will prevent confusing cases such as a variable named `strange` that starts with `str` but it is just a part of the word not a prefix, so, it will not be considered of type `String`, unless you named it `strStrange`, or `strAnge` :D.
This feature makes it easy to work with complex expressions , function parameters, and ForEach iteration variables, as sVB can't infer their types directly. 

34. sVB now fully supports working with dates:
 * The `Date` type provides methods to create dates, read and modify date parts, and add values to them.
 * Date variables can use `Date` methods as extension methods.
 * You can use date literals directly in code. Ex:
```vb
d = #1/31/2022#
TextWindow.WriteLine(d.LongDate)
```

In the above example, we used the English date format, where the month appears before the day. This is a must as long as you use month number in the `# #` literals. You can move the caret to the date literal, and the help popup window will show the date value in your system culture.
But if you write the month name, you can put it in any order!
```vb
d = #1 Jan 2022#
TextWindow.WriteLine(d.LongDate)
```

The date literal can also contain the time, like:
```
d = #12/27/2020 9:10:6 AM#
```

and if you omit the date part, the today date will be used:
```vb
d = #15:10:6.123#
TextWindow.WriteLine(d.LongDate)
```

note that the ".123" is the milliseconds part, and here we used the 24 hour clock, so, we don't need to use the AM/PM part.
In short: these are the exact same date formats used in VB.NET, and you can review them in MS docs.

* You can also use TimeSpan literals directly in code, which is a specific feature for sVB that doesn't exist in VB.NET. Ex:
```VB
ts = #+1.10:14:30#
TextWindow.WriteLine(ts.TotalHours)
```

This code will show the result `34.24`, as the time span (duration) contains 1 day, 10 hours and about one quarter of an hour, so, the TotalHours property gives us approximately `34.24`.
Note that time span literal must start with a `+` or `-`, to distinguish it from date literal. The rest of the time span format is  similar to VB.NET. It can contain only days, hours, minutes, seconds, and milliseconds. Ex:
```VB
ts = #-1000.10:14:30.500#
years = ts.TotalDays / 365
TextWindow.WriteLine(years)
```

the above negative time span contains approximately `-2.74` years. There is no built in TotalMonths nor TotalYears in the Date class, as a monthe can contain 29, 30, or 31 days, and a year can contain 365 or 366 days, so, it is up to you to do the math according to the rules you see fit tour needs.

* The Date class contains `Add` and `Subtract` methods, but you can do these operations directly using `+` and `-`. The trick here is that sVB stores dates and time spans as `ticks`. A tick is 1 over 10 million of a second (1 second = 10 million ticks). You can get the total ticks of a date or a time span by calling the `Date.GetTicks()` method, or the `Ticks` extension property. So, when using math operator with date and time span, sVB treats thesm as normal numbers. You can even multiply 2 dates but of course the result is meaningless :D.
The following code show you 2 different ways to subtract 1000 days from the today's date:
```VB
d1 = Date.Now - #+1000.0:0#
TextWindow.WriteLine(d1)

d2 = Date.Now
TextWindow.WriteLine(d2.AddDays(-1000))
```

* You can also use comparison operators like `>`, `<` and `=` to compare tow dates or time spans. Ex:
```vb
D1 = Date.FromCulture("22/9/2022", "ar-EG")
D2 = Date.Now
If D1.ShortDate = D2.ShortDate Then
   TextWindow.WriteLine("In the present")
ElseIf D1 > D2 Then
   TextWindow.WriteLine("In the future")
Else
   TextWindow.WriteLine("In the past")
EndIf
```

* sVB toolbox now has a `DatePicker` control, to allow the user to select dates from a dropdown calendar. Use the `SelectedDate` to get or set the select date in the control, and use the `OnSelection` event to interact with the user after selection a date from the calendar.

35. You can change controls font properties from code. Previously, this was only available via the form designer, but now every control has `FontName`, `FontSize`, `FontBold` and `FontItalic` properties. The auto-completion list will show font names available on your system when you setting the value of the FontName.

36. The TextBox control now has SelectionStart, SelectionLength, SelectedText, and CaretIndex properties.

37. All controls now have the Tag property to allow you to store additional data related to the control.

38. The Array class now has Find and Join methods.


39. Starting from sVB 1.9, you can compile a project!
You can design many forms in the form designer, save them to the same folder, which will become the project folder. When you open any form of this project and run it, sVB will compile all the forms into one exe (that will have the name of the folder/project).
You can show form2 (for example) from form1 using this code:
```vb
   form2 = Forms.ShowForm("form2", {1, 2, 3})
   form2.BackColor = Colors.AliceBlue
```

The ShowForm method will do the following:
* Load the design of the form2 from its xaml file.

* pass the argsArr data sent to its second parameter to the ArgsArr property of the form, so you can use it as you want. The argsArr can be a single value, and array of values, or a dynamic object with dynamic properties, so, you can pass any data you want between the forms.

* Execute the code written in the global area of the code file of form2. You can use Form2.ArgsArr in this global area to initialize the form. Ex:
```VB
  TextBox1.Text = Me.ArgsArr
```

Note that global code is executed only when the form is opened for the first time, or after it is closed then re-opened. It will not be executed if you hided or minimized the form then showed it again.
* Show Form2.

* Fire the OnShown Event of the form. You can use it also to initialize the form:
```vb
Sub Form2_OnShown()
   data = Me.ArgsArr
   TextBox1.Text = data[3]
EndSub
```

This event has an advantages over using the global code area to initialize the form, that it will be executed every time you call Forms.ShowForm, so, you can use the passed argsArr data every time you show the form even it is still open.
For a simple sample, see the `Random Buttons 2` sample in the samples folder. It is a modified version of the `Random Buttons` sample, which uses code to define and show another form. In the new version, the second form is designed by the form designer.

Note that the form you run the program from will be main form of the project (the startup form). This allows change the startup form as you want by just open the form code and press F5, so you can easily test project forms.

40. Show a Dialog:
You can show the form as a dialog (modal window), by calling `Forms.ShowDialog` instead of Forms.ShowForm. The dialog window stops executing the code until the user closes it, so, you can't access the dialog form while it is displayed. So, you need to pass all the date throw the argsArr argument, and process it in the in the dialog form.
When you show a dialog, you want to know what action the user took to close the dialog. He may accept what you offer him by clocking the `OK` or `Yes` buttons, refuse by clicking `No` button, or cancels the operation by clicking the `Cancel` button of closing the form. So, you need to indicate such actions when you write these buttons code, by setting the suitable value for the Form.DialogResult property of the form:
```vb
LblMsg.Text = Me.ArgsArr

Sub BtnYes_OnClick()
   Me.DialogResult = DialogResults.Yes
   Me.Close()
EndSub

Sub BtnNo_OnClick()
   Me.DialogResult = DialogResults.No
   Me.Close()
EndSub

Sub BtnCancel_OnClick()
   Me.Close()
EndSub
```

Note that the `DialogResults` type contains the names of famous dialog buttons, and it has a nice auto completion suuport in the code editor, but you can not use it and use any other names you want.
`Cancel` is the default value of the `DialogResult`, so we disn't need to set it in the `BtnCancel_OnClick()`.
Now, how can we use the `DialogResult` value in the form that showed the dialog?
It is simple: the `DialogResult` value will be the return value from the `Forms.ShowDialog`, so, it is easy to use it like this:
```vb
Sub Button1_OnClick()
   result = Forms.ShowDialog(
      "form2", "Do you want to save the changes?")
   
   If result = DialogResults.Yes Then
      TextBox1.Text = "User accepted to save changes."
   ElseIf result = DialogResults.No Then
      TextBox1.Text = "User refused to save changes."
   Else
      TextBox1.Text = "User canceled the operation."
   EndIf
EndSub
```

You can try this code in the `Show Dialog` sample in the samples folder.

41. Project Explorer:
starting from sVB 1.9, the form designer shows a list of project files (the files exists in the same directory of the current opened form). You can use this list to rename the file or delete it directly. 
This list is different than the `open forms list`, which show the form names of all opened forms even they don't belong to the same project (folder). You can use this list to close the opened form (this will not delete if from its project) or to change the name of the form (this will not change its file name). This list is more like the VS.NET tabs that shows the form design or code files.

42. Starting from sVB 2.0, each project can contain a Global.sb file. You can create it just by double-clicking the Global.sb item in the project explorer.
In this file, you can declare global variables, subroutines and functions that you can use in any form in the project via the `Global` type, such as `Global.DoSomething()`
See the `Show Random Buttons 3` and `Show Dialogs` apps in the samples folders.

43. The toolbox got a new `ComboBox` control.

44. Controls got `RightToLeft`, `ToolTip` and `Errors` properties. 
The Error property sets the error message to display in the tooltip of the control while its borders becomes red. You can restore the normal state of the control by setting the Error  
property to an empty string.

45. Controls also got the `OnGotFocus` and `OnLostFocus` events, so, you can use the `OnLostFocus` event with the `Error` property to provide a validation logic for input controls. Each control also has the `Validate` method that fires the `OnLostFocus` event to execute your validation logic (if exists), then checks the Error property and returns `True` if it is empty. And to make things easier for you, the `Validate` method of the form calls the Validate method of each control on the form. If any controls has errors, the process stops and the focus goes to this contols with a beep sound. So, you should call `Me.Validate` before you execute you app logic. Ex:
```VB
Sub Button1_OnClick()
   If Me.Validate() Then
      Me.ShowMessage("OK", "")
   EndIf
EndSub
```

But if you want to do anything else when a control input is not valid, you can validate each control your self:
```vb
Sub Button1_OnClick()
   ForEach control1 In Me.Controls
      If control1.Validate() = False Then
         Sound.PlayBellRing()
         control1.Focus()
         ' Do more things here if you want
         Return
      EndIf
   Next
   
   Me.ShowMessage("OK", "")
EndSub
```

For a full example, see the `Validation` app in the samples folder.

46. Starting from V2.1, sVB tries to infer the type of an expression, and the type of the function return value.

47. sVB 2.2 supports creating custom complex shapes using the GeometricPath type.
For example, you can draw a hex polygon shape, add it to the shapes, then rotate it by this code:
```vb
GraphicsWindow.BrushColor = Colors.AliceBlue
GraphicsWindow.PenColor = Colors.Red
GraphicsWindow.PenWidth = 3

' Crate a figure to draw a new custom shape
GeometricPath.CreateFigure(100, 100, True)
GeometricPath.AddLineSegment(50, 150, True)
GeometricPath.AddLineSegment(100, 200, True)
GeometricPath.AddLineSegment(200, 200, False) ' using `False` hides the line
GeometricPath.AddLineSegment(250, 150, True)
GeometricPath.AddLineSegment(200, 100, True)

Sh1 = Shapes.AddGeometricPath()
Shapes.Rotate(Sh1, 45)
```

And you may use curves and arcs not just lines:
```
GraphicsWindow.BrushColor = Colors.Transparent
GraphicsWindow.PenColor = Colors.Blue
GraphicsWindow.PenWidth = 5

' Create a new empty geometric path
GeometricPath.CreatePath()
GeometricPath.CreateFigure(100, 100, True)
GeometricPath.AddLineSegment(50, 150, True)
GeometricPath.AddArcSegment(100, 200, 20, 30, 30, False, True, True)
GeometricPath.AddLineSegment(200, 200, True)
GeometricPath.AddBezierSegment(210, 210, 230, 230, 250, 150, True)
GeometricPath.AddQuadraticBezierSegment(230, 110, 200, 100, True)
Sh2 = Shapes.AddGeometricPath()
Shapes.Move(Sh2, 130, 100)
```


You can also combine some basic shapes in one path:
```vb
GraphicsWindow.BrushColor = Colors.Yellow
GraphicsWindow.PenColor = Colors.Black
GraphicsWindow.PenWidth = 1

GeometricPath.CreatePath()
GeometricPath.AddLine(100, 100, 400, 400)
GeometricPath.AddRectangle(100, 100, 200, 200)
GeometricPath.AddEllipse(150, 150, 100, 100)
GeometricPath.AddTriangle(300, 300, 400, 200, 400, 400)
Sh3 = Shapes.AddGeometricPath()
Shapes.Move(Sh3, 300, 0)
Shapes.Rotate(Sh3, 45)
```

You can see this code in action in the `Geometric Path` app in the samples folder. And this is the 3 shapes that the above code draws:
![image](https://user-images.githubusercontent.com/48354902/195464252-203037b8-b057-4253-b7c6-bdc34e94ffe6.png)

You can also add the geometric path to a label, by calling the `AddGeometricPath()` method of this label:
```vb
Label1.AddGeometricPath(
    Colors.Red, ' Pen color
    2, ' Pen width
    Colors.Yellow ' Brush color
)
```

This allows you to add complex shpes on the form with code, and program their events.
See the `Geometric Path2` in the samples folder. It adds the same shapes of the previous sample to the form, and allow you to drag them by the mouse.

48. The toolbox now contains a CheckBox control.
Note that the Checked property is a tri-state property, as it accepts 3 valid values:
* True: to check the box.
* False: to uncheck the box.
* empty string `""`: to dim the box to indicate an indeterminate state.
You can also allow the user to switch by these tri-states while he is clicking the checkbox, by setting the `AllowTriState` property to `True`, but this property doesn't affect your ability to set the indeterminate state from code at anytime.
For more info, see the `CheckBox Sample` app in the samples folders.

49. The toolbox now contains a RadioButton control.
Radio buttons work in groups. Buttons belong to the same group can have only one button selected at a time, so, you can have many groups on the form to let the user choose between deferent sets of options. 
You can combine some radio buttons into one group by setting the `GroupName` property of each of then to the same name like `Group1`. Or as an easier alternative, you can use the Form Designer to group radio buttons by selecting them, right-click, and click the `Group` command from the context menu. This will combine the radio buttons into one border so you can drag them together, and also will set the GroupName property of each of the to the same name, so they work as a group.
For more info, see the ` Radio Button sample` app in the samples folders.

50. Label, TextBox, Button, CheckBox and RadioButton controls got an `Underlined` property, that you can set to True to draw a line under the text displayed in these controls.
They also got a `WordWrap` property, that you can set to True to allow long line to continue to the next line. The default is False.

51. You can now use the Append methods to add a formatted text to the Label. These methods are:
* Append: Adds a normal text to the end of the label text. This text will be formatted according the properties of the label like BackColor, ForeColor, Underlined and other Font properties.
* AppendLine: Similar the Append method, but it adds a new line after the text.
* AppendBold: Appends a text to the label with a bold font.
* AppendItalic: Appends a text to the label with an italic font.
* AppendBoldItalic: Appends text to the label with a bold and italic font.
* AppendUnderlined: Appends a text to the label with a line drawn under it.
* AppendWithFontEffects: Allows to control the Bold, Italic, underline effects of the appended text, by sending `True`, `False` or `""` to the corresponding parameters. Not that sending empty string means using the original label property to set this effect. For example, if you send "" to the isBold parameter, the value Label.FontBold property will be used to draw the text. Ex:
```VB
Label1.AppendWithFontEffects(
    "Hello", 
    "",              ' ignore isBold param and use that of the label
    True,         ' isItalic = True
    False         ' isUnderLined = False
)
```

* You can control the Font name and size by calling the ` AppendWithFontName`, ` AppendWithFontSize` or ` AppendWithFontNameAndSize` methods. Note that sending "" to font name or size will draw the text with the original label properties.

* The `AppendWithFont` methods allows you to set font name, size, and effects together.

* You can control the fore and back colors by calling the ` AppendWithForecolor`, ` AppendWithBackcolor` or ` AppendWithColors` methods. Note that sending "" as the fore color or the back color will draw the text with the original label properties.

* You can also append a hyper link to the label, by calling the `AppendLink`, `AppendBoldLink`, `AppendItalicLink` or ` AppendBoldItalicLink`, passing to the text of the link and a url to navigate to when the user clicks it. You can supply a local folder or file path or an IP address for the url parameter. When the user clicks the link, the folder or the file will be oppened (using the default program for this type of files) and the IP address will be navigated to in the default browsr on the user PC. If url is bad of file is not found, a blank page will be opened in the browser.

* The `AppendFormatted` method combines all the above method. It can add a formatted text, or a formatted link if you send a non-empty string to the url parameter. In fact all other methods calls the `AppendFormatted` method, with ignored parameters set to `""`, so, they are just shortcuts to make your code shorter.

Note that appending text and changing font size can make the label width and height smaller than showing all the text, so, you may need to use the Label.Width and Label.Height to adjust the label size afterwards. You can also make one or both of them auto-size by setting it to `-1`. If you set the width to a fixed length (not -1), then the longer text will be wrapped to next lines, so you need to enlarge the Height or make it auto-size (`= -1`).
For more info, see the `Label Formats` app in the samples folder.

52. More enhancements to the intellisense. For example, the auto completion list will offer the color names when you are in a pos of a color argument of a method. This will happen just you write the `,` in the argument list. If you hide the list and want to show it again, just press Ctrl+Space to show the color names again.
Also, you can commit the suggested name from the completion list to a new line, just press Ctrl+Enter. This is helpful if you are splitting the arguments over multi-lines, so, when you write one argument then `,` and the auto completion appears to offer color, font or key names, just choose the name and press Ctrl+Enter to add it to the next line.

53. Controls have `FitContentSize`, `FitContentWidth` and `FitContentHeight` to allow you to adjust the contol width and height to fit the contents. These methods will not make the control auto-sized, as their effect is for one time only.
See the `Fit size` app in the samples folder.

# Create a code library!
For the first time, it is now possible to use an sVB app as a library! Follow these instructions:
1. Create a sVB project with a global file. Choose a suitable name for the folder that you save the project files to (such as MyLib), because the name of this folder will be the name of your library. Don't use spaces nor symbols. 
2. Add variables, subroutines, and functions to the global files. These are the members that you can access from the lib. But if you want to declare some private members to use them inside the global module only, you can simply give these members names that starts with `_` such as `_count` and `_GetNames()`.
3. Add comments for each variable, subroutine, parameter, and return value. These comments will be saved as the documentation for your lib, and will be shown in popup help in sVB when you use this lib.
4. You can also add a comment at the beginning of the global file, to be used as the documentation for the Lib Type.
5. The project can contain as many forms as you need, but you must choose unique names when saving them to the project folder. Form1.xaml, Form2.xaml and Form3.xaml can cause troubles later, so, if you must, name them MyLib_Form1.xaml, MyLib_Form2.xaml and MyLib_Form3.xaml. Don't rename the files manually from windows explorer, and use the sVB project explorer to rename them, to do necessary changes to the .sb.gen file.
6. Run the project to create the exe file in the Bin folder in the project folder.
7. Now you can copy the Bin folder and paste in the `sVB\Bin\Lib` folder, then rename it to the name of your lib such as `MyLib` in this example. The name of this folder is not important, but Bin is not a suitable name, and of course you can't add tow libraries with folders named Bin because they will be merged, which can cause troubles later!
8. Restart sVB, and in the code editor write the name of your library such as MyLib, then press dot. The auto completion list will show the members you declared in the global file and can use them. If you add comments to those members, you will get help info about them while typing.
It is so simple. I already applied it on the `Dialogs` and `Geometrics` projects in the samples folder, and added the two libraries to the Lib folder of the sVB realease, so, you can now use this code in any form that have a label:
```vb
Name = Dialogs.InputBox("Enter your name")
Geometrics.CreateHexagon(Label1, Colors.AliceBlue, 7, Colors.Brown)
Geometrics.AllowDrag(Label1)
```

So, now you can create reusable code, and write your own libraries for sVB. In the past, this was available only by using C# and VB.NET to create SB and sVB libraries!
Have fun.
