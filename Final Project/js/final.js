$(document).ready(function(){
	
	console.log("Whether we wanted it or not, we've stepped into a war with the Cabal on Mars. So let's get to taking out their command, one by one. Valus Ta'aurc. From what I can gather he commands the Siege Dancers from an Imperial Land Tank outside of Rubicon. He's well protected, but with the right team, we can punch through those defenses, take this beast out, and break their grip on Freehold.");
	
    $(".datePicker").datepicker();
    
    $(".radioset").buttonset();
    
    $("#position").selectmenu();
    $("#discharge").selectmenu();
    
    
    $("#hideme1").hide();
    $("#hideme2").hide();
    $("#hideme3").hide();
    $("#hideme4").hide();
    $("#militaryService").hide();
    
    $("#reference2").hide();
    $("#reference3").hide();
    
    $("#employer2").hide();
    $("#employer3").hide();
    
    $("#citizen2").click(function() {
        $("#hideme1").slideDown("fast");
    });
    
    $("#citizen1").click(function() {
        $("#hideme1").slideUp("fast");
    });
    
    
    $("#military1").click(function() {
        $("#militaryService").slideDown("fast");
    });
    
    $("#military2").click(function() {
        $("#militaryService").slideUp("fast");
    });
    
    
    $("#worked1").click(function() {
        $("#hideme2").slideDown("fast");
    });
    
    $("#worked2").click(function() {
        $("#hideme2").slideUp("fast");
    });
    
    
    
    $("#felony1").click(function() {
        $("#hideme3").slideDown("fast");
    });
    
    $("#felony2").click(function() {
        $("#hideme3").slideUp("fast");
    });
    
    $("#showMore1").button();
    $("#showMore1").click(function() {
        $("#employer2").slideToggle("fast");
        $("#employer3").slideToggle("fast");
    });
    
    $("#showMore2").button();
    $("#showMore2").click(function() {
        $("#reference2").slideToggle("fast");
        $("#reference3").slideToggle("fast");
    });
    
	$("#confirm").dialog({
		title: "Submit application?",
		autoOpen: false,
		buttons: [
			{
			  text: "I agree",
			  icon: "ui-icon-check",
			  click: function() {
				$( this ).dialog( "close" );
			  }
			},
			{
			  text: "Cancel",
			  icon: "ui-icon-cancel",
			  click: function() {
				$(this).dialog( "close" );
			  }
			}
		]
	});
	
	
	$("#testForm0").validate({
        rules: {
            state: {
                required: true, rangelength:[2,2]
            },
            zip: {
                required: true, rangelength:[5,5]
            },
            phone: {
                required: true, rangelength:[10,15]
            },
            ssn: {
                required: true, rangelength:[9,9]
            }
        }, // End rules
        messages: {
            state: {
                required: "Enter your state",
                rangelength: "State must be 2 characters long."
            },
            zip: {
                required: 'Please enter a zip code.',
                rangelength: 'Zip must be 5 characters long.'
            },
            phone: {
                required: 'Please enter a phone number.',
                rangelength: 'Phone number must be 10 to 15 characters long.'
            },
            ssn: {
                required: 'Please enter your Social Security number.',
                rangelength: 'SSN must 9 characters long.'
            }
        }, // End messages
        
        errorPlacement: function(error, element) {
            if ( element.is(":radio") || element.is(":checkbox")) {
                error.appendTo( element.parent());
            } else {
                error.insertAfter(element);
            }
        } // End Errors
        
    });
	$("#testForm1").validate({
        rules: {
            employerPhone: {
                rangelength:[10,15]
            }
        }, // End rules
        messages: {
            employerPhone: {
                rangelength: 'Phone number must be 10 to 15 characters long.'
            }
        }, // End messages
        
        errorPlacement: function(error, element) {
            if ( element.is(":radio") || element.is(":checkbox")) {
                error.appendTo( element.parent());
            } else {
                error.insertAfter(element);
            }
        } // End Errors
        
    });
	$("#testForm2").validate({
        errorPlacement: function(error, element) {
            if ( element.is(":radio") || element.is(":checkbox")) {
                error.appendTo( element.parent());
            } else {
                error.insertAfter(element);
            }
        } // End Errors
    });
	$("#testForm3").validate({
        rules: {
            refPhone: {
                rangelength:[10,15]
            }
        }, // End rules
        messages: {
            refPhone: {
                rangelength: 'Phone number must be 10 to 15 characters long.'
            }
        }, // End messages
        
        errorPlacement: function(error, element) {
            if ( element.is(":radio") || element.is(":checkbox")) {
                error.appendTo( element.parent());
            } else {
                error.insertAfter(element);
            }
        } // End Errors
        
    });
	$("#testForm4").validate();
	
	$("#tabs").tabs({
		
		disabled: [ 1, 2, 3, 4 ],
		active: 0,
		heightStyle: "fill"
		
	});
	
	$("#tab0submit").button();
	$("#tab0submit").click(function(){
		
		if( $("#testForm0").valid() ){
			$("#tabs").tabs({
				disabled: [ 2, 3, 4 ],
				active: 1
			});
		}
		
	});
	
	$("#tab1submit").button();
	$("#tab1submit").click(function(){
		
		if( $("#testForm1").valid() ){
			$("#tabs").tabs({
				disabled: [ 3, 4 ],
				active: 2
			});
		}
		
	});	
	
	$("#tab2submit").button();
	$("#tab2submit").click(function(){
		
		if( $("#testForm2").valid() ){
			$("#tabs").tabs({
				disabled: [ 4 ],
				active: 3
			});
		}
		
	});	
	
	$("#tab3submit").button();
	$("#tab3submit").click(function(){
		
		if( $("#testForm3").valid() ){
			$("#tabs").tabs({
				disabled: [  ],
				active: 4
			});
		}
		
	});	
	$("#tab4submit").button();
	$("#tab4submit").click(function(){
		
		if( $("#testForm0").valid() && $("#testForm1").valid() && $("#testForm2").valid() && $("#testForm3").valid() ){
			
			// bring up a jqueryui confirm dialog
			$("#confirm").dialog( "open");
		}
		
	});
    
});