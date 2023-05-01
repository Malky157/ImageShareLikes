$(() => {
    $("#like-button").on("click", function () {
        let id = $("#image-id").val()
        
        $.post("/home/incrementlikes", { id }, function () {
         
            $.post("/home/setsession", { id }, function () {
                window.location.reload();
            }) 
        })
    })
    
    setInterval(() => {
        let id = $("#image-id").val()
        $.get('/home/GetLikes', { id }, currentLikes => {
            $("#likes-count").text(currentLikes);            
        })
    }, 2000)
    
})