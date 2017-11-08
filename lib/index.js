'use strict';
const edge = require('edge');
const events = require('events');
const adodb = new Promise((res,rej) => {
    edge.func(require('path').join(__dirname, 'edge_adodb.dll'))
    (null, (err,val)=>{
        if(err)throw err;
        res(val);
    });
})

function Query(connection, query){
    return new Promise(async (res,rej)=>{
        (await adodb).Query({connection:connection,query:query},(err,val)=>{
            if(err) return rej(err);
            return res(val);
        });
    });
}

function Execute(connection, query){
    return new Promise(async (res,rej)=>{
        (await adodb).Execute({connection:connection,query:query},(err,val)=>{
            if(err) return rej(err);
            return res(val);
        });
    });
}

class Connection {
    constructor(connection) {this.connection = connection}
    query(query){
        return Query(this.connection, query);
    }
    execute(query) {
        return Execute(this.connection, query);
    }
}

class EventConnection {
    constructor(connection) {this.connection = connection}
    query(query){
        let ev = new events.EventEmitter();
        Query(this.connection, query)
        .then((res)=>ev.emit('done', res))
        .catch((err)=>ev.emit('fail', err));
        return ev;
    }
    execute(query) {
        let ev = new events.EventEmitter();
        Execute(this.connection, query)
        .then((res)=>ev.emit('done', res.result))
        .catch((err)=>ev.emit('fail', err));
        return ev;
    }
}

exports.open = (connection) => {
    return new EventConnection(connection);
}

exports.connect = (connection) => {
    return new Connection(connection);
}
/*
helloWorld("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\projects\\el-yta\\sida\\Fakt000.Mdb;", (err, res)=>{
    if(err)console.log(err.message);
    console.log(err, res);
});
*/