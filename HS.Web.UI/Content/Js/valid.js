IsPhoneNumber: function (phone) {
   
    try {
       
        if (phone != undefined && phone != null && phone != "")
        {
            phone = phone.replace(/[-  ]/g, '');
                
                
            if (phone.length == 10) {
                   
                return true;
            }
            else if (phone.length > 10) {
                   
                return false;
            }
            else if (phone.length < 10) {
                  
                return false;
            }
            else{
                return false;
            }
                
        }
    }
    catch (e) {
        return false;
    }
}