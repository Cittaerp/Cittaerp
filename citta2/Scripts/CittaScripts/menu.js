 
 $(document).ready(function () { 
 
 $("#menu-toggle").mouseover(function () {
        $("#sidebar").show();

        $("#sidebar").mouseleave(function () {
            $("#sidebar").fadeOut();
        });
 });

 $(".dropdown-submenu a.dropdown-toggle").on("click", function (e) {
     $(this).next('ul').toggle();
     e.stopPropagation();
     e.preventDefault();
 });
	})