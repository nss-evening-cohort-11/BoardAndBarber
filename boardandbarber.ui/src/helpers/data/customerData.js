
const getAllCustomers = () => new Promise((resolve,reject) => {
    resolve([   
        {
            id:1,
            name:"nathan",
            birthday:"5/27/1986", 
            favoriteBarber:"Jimbo", 
            notes: "High and tight" 
        },
        {
            id:2,
            name:"Billy",
            birthday:"3/27/1986", 
            favoriteBarber:"Kimbo", 
            notes: "Low and loose" 
        }])
} );

export default {getAllCustomers};