# Edge-ADODB
>A simple ADODB driver runing through Edgejs.
>
>[![NPM Version][npm-image]][npm-url]

### Install
[![NPM](https://nodei.co/npm/edge-adodb.png)](https://nodei.co/npm/edge-adodb/)

### Introduction:
```js
const adodb = require('edge-adodb');
const connection = adodb.connect('Provider=Microsoft.ACE.OLEDB.12.0;Data Source=edge-adodb.Mdb;');

connection.execute('INSERT INTO Users(UserName, UserSex, UserAge) VALUES ("Newton", "Male", 25)')
    .then(data=>{
        console.log(data);
    })
    .catch(err=>{
        console.error(err);
    });
```

## API:
### Promise based
`adodb.connect(connectionString)`
>returns EventEmitter based adodb connection

`connection.query(sql)`
>returns promise

`connection.execute(sql)`
>returns promise

### EventEmitter based
`adodb.open(connectionString)`
>returns EventEmitter based adodb connection

`connection.query(sql)`
>returns EventEmitter that emits done or fail depending on success

`connection.execute(sql)`
>returns EventEmitter that emits done or fail depending on success

[npm-image]: https://img.shields.io/npm/v/edge-adodb.svg?style=flat-square
[npm-url]: https://www.npmjs.org/package/edge-adodb