import React, {Component} from "react";
import {appsettings} from "../../App/appsettings"
import Cookies from 'js-cookie'
import { Page } from "../Page";
import Select from 'react-select';

export class Events extends Component{
    constructor(props){
        super(props);
        this.state = {
            events:[],
            modalTitle:"",
            Id:0,
            Name:"",
            Description:"",
            LayoutId:0,
            TimeStart:"",
            TimeEnd:"",
            ImageUrl:"",
            nextEvents:[],
            currentEventDetails:"",
            EventSeatId:0,
            options:[],
            roles:this.props.roles
        }
        Page.number = 1;
    }

    getNextPage(){
        var nextPageNumber = Page.number;
        nextPageNumber++;
        var urlNext = new URL(appsettings.EventManagerApiAddress + 'events/getevents'),
        paramsNext = {pageNumber:nextPageNumber}
        Object.keys(paramsNext).forEach(key => urlNext.searchParams.append(key, paramsNext[key]))
        fetch(urlNext, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': this.checkForCookie()
            }
        }).then(response => response.json()).then(data => this.setState({ nextEvents: data }));
    }

    checkForCookie(){
        var jwtToken = Cookies.get('JwtTokenCookie');
        if(jwtToken !== undefined){
            return jwtToken;
        }
        return '';
    }

    nextPage(){
        Page.number++;
        this.refreshList();
    }

    firstPage(){
        Page.number = 1;
        this.refreshList();
    }

    previousPage(){
        Page.number--;
        this.refreshList();
    }

    changeName = (e) =>{
        this.setState({
            Name:e.target.value
        });
    }

    changeDescription = (e) =>{
        this.setState({
            Description:e.target.value
        });
    }

    changeLayoutId = (e) =>{
        this.setState({
            LayoutId:e.target.value
        });
    }

    changeTimeStart = (e) =>{
        this.setState({
            TimeStart:e.target.value
        });
    }

    changeTimeEnd = (e) =>{
        this.setState({
            TimeEnd:e.target.value
        });
    }

    changeImageUrl = (e) =>{
        this.setState({
            ImageUrl:e.target.value
        });
    }

    changeEventSeatId = (e) =>{
        this.setState({
            EventSeatId:e.value
        })
    }

    refreshList(){
        var url = new URL(appsettings.EventManagerApiAddress + 'events/getevents'),
            params = {pageNumber:Page.number}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
        fetch(url, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': this.checkForCookie()
            }
        }).then(response => response.json()).then(data => this.setState({ events: data}));
        this.getNextPage();
    }

    componentDidMount(){
        this.refreshList();
    }

    onCreateClick(){
        fetch(appsettings.EventManagerApiAddress + 'events/create',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: 0,
                Name:this.state.Name,
                Description:this.state.Description,
                LayoutId:this.state.LayoutId,
                TimeStart:this.state.TimeStart,
                TimeEnd:this.state.TimeEnd,
                ImageUrl:this.state.ImageUrl,
            })
        }).then(response => {
            if (!response.ok){
                response.text().then(data => alert(data))
            }
        }).then(()=>{
            this.refreshList();
        })
    }

    onEditClick(id){
        fetch(appsettings.EventManagerApiAddress + 'events/edit/' + id,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: id,
                Name:this.state.Name,
                Description:this.state.Description,
                LayoutId:this.state.LayoutId,
                TimeStart:this.state.TimeStart,
                TimeEnd:this.state.TimeEnd,
                ImageUrl:this.state.ImageUrl,
            })
        }).then(response => {
            if (!response.ok){
                response.text().then(data => alert(data))
            }
        }).then(()=>{
            this.refreshList();
        })
    }

    createClick(){
        this.setState({
            modalTitle:"Add event",
            Name:"",
            Description:"",
            LayoutId:0,
            TimeStart:"",
            TimeEnd:"",
            ImageUrl:"",
            Id:0
        });
    }

    editClick(event){
        this.setState({
            modalTitle:"Edit event",
            Id:event.id,
            Name:event.name,
            Description:event.description,
            LayoutId:event.layoutId,
            TimeStart:event.timeStart,
            TimeEnd:event.timeEnd,
            ImageUrl:event.imageUrl,
        });
    }

    deleteClick(id){
        if(window.confirm('Are you sure?')){
            fetch(appsettings.EventManagerApiAddress + 'events/delete/' + id,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
            }}).then(response => {
                if (!response.ok){
                    response.text().then(data => alert(data))
                }
            })
            .then(()=>{
                this.refreshList();
            })
        }
    }

    buyClick(event){
        fetch(appsettings.EventManagerApiAddress + 'events/details/' + event.id, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ currentEventDetails: data })); 
        this.setState({ Id: event.id }, () => {
            var urlForFreeSeats = new URL(appsettings.EventManagerApiAddress + 'eventseats/getfreeseats/'),
                            paramsForFreeSeats = {eventId: event.id}
                        Object.keys(paramsForFreeSeats).forEach(key => urlForFreeSeats.searchParams.append(key, paramsForFreeSeats[key]))
            fetch(urlForFreeSeats, {
                method:"GET",
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
                }
            }).then(response => response.json()).then(data => {
                var options = [];
                data.forEach(eventSeat => options.push({ value: eventSeat.id, label: eventSeat.id}))
                this.setState({options: options})
            }); 
        })
    }

    onBuyClick(){
        var currentEvent = this.state.currentEventDetails;
        var eventSeatId = this.state.EventSeatId;
        for (var i = 0; i < currentEvent.eventAreas.length; i++){
            for (var j = 0; j < currentEvent.eventAreas[i].eventSeats.length; j++){
                // eslint-disable-next-line
                if (currentEvent.eventAreas[i].eventSeats[j].id == eventSeatId){
                    let url = new URL(appsettings.PurchaseFlowApiAddress + 'events/buy'),
                        params = {eventSeatId:eventSeatId, price: currentEvent.eventAreas[i].eventArea.price}
                    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
                    fetch(url, {
                        method:"GET",
                        headers:{
                            'Accept':'application/json',
                            'Content-Type':'application/json',
                            'authorization': Cookies.get('JwtTokenCookie')
                        }
                    }).then(response => response.json()).then(data => {
                        if(window.confirm('Are you sure to buy this event seat for ' + data.price + '$?')){
                            fetch(appsettings.PurchaseFlowApiAddress + 'events/buy/',{
                                method:'PUT',
                                headers:{
                                    'Accept':'application/json',
                                    'Content-Type':'application/json',
                                    'authorization': Cookies.get('JwtTokenCookie')
                                },
                                body:JSON.stringify({
                                    Id:data.id,
                                    EventSeatId:data.eventSeatId,
                                    UserId:data.userId,
                                    Price:data.price
                                })
                            }).then(response => {
                                if(!response.ok){
                                    alert("Not enough money!")
                                }
                            })
                        }
                    })
                    break;
                }
            }
        }
    }

    render(){
        const {
            events,
            modalTitle,
            Id,
            Name,
            Description,
            LayoutId,
            TimeStart,
            TimeEnd,
            ImageUrl,
            nextEvents,
            roles,
            options
        }=this.state;

        return(
            <div className="container">
                {roles.includes("admin") || roles.includes("event manager")?
                <p>
                    <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalWindow"
                        onClick={()=>this.createClick()}>
                            Add event
                    </button>
                </p>:null}

                {events.map(event =>
                    <div className="col" key={event.id}>
                    <figure className="row">
                        <img className="col" src={event.imageUrl} height="300" alt="eventPoster" />
                        <div className="col">
                            <h3>{event.name}</h3>
                            <h5>Описание: {event.description}</h5>
                            <h5>Начало события: {event.timeStart}</h5>
                            <h5>Конец события: {event.timeEnd}</h5>
                            {roles.includes("admin") || roles.includes("event manager") || roles.includes("user") || roles.includes("venue manager")?
                            <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                                        data-bs-target="#modalWindowForBuy" onClick={()=> this.buyClick(event)}>
                                Buy
                            </button>:null}
                            {roles.includes("admin") || roles.includes("event manager")?
                            <div>
                                <hr/>
                                <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                                        data-bs-target="#modalWindow" onClick={()=>this.editClick(event)}>
                                    Edit
                                </button>
                                <button type="button" className="btn btn-danger" onClick={()=>this.deleteClick(event.id)}>
                                    Delete
                                </button>
                            </div>:null}
                        </div>
                    </figure>
                    <hr/>
                </div>
                )}

                <div>
                    {Page.number!==1?
                        <button type="button" className="btn btn-primary" onClick={() => this.firstPage()}>1</button>:null}
                    {Page.number > 1?
                        <button type="button" className="btn btn-primary" onClick={() => this.previousPage()}>&lt;</button>:null}
                    {nextEvents.length !== 0?
                        <button type="button" className="btn btn-primary" onClick={() => this.nextPage()}>&gt;</button>:null}
                </div>

                <div className="modal fade" id="modalWindow" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{modalTitle}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">Name</span>
                                    <input type="text" className="form-control" value={Name} onChange={this.changeName}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">Description</span>
                                    <input type="text" className="form-control" value={Description} onChange={this.changeDescription}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">LayoutId</span>
                                    <input type="number" className="form-control" value={LayoutId} min={1} onChange={this.changeLayoutId}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">TimeStart</span>
                                    <input type="datetime-local" className="form-control" value={TimeStart} onChange={this.changeTimeStart}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">TimeEnd</span>
                                    <input type="datetime-local" className="form-control" value={TimeEnd} onChange={this.changeTimeEnd}/>
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text ">ImageUrl</span>
                                    <input type="text" className="form-control" value={ImageUrl} onChange={this.changeImageUrl}/>
                                </div>
                                {Id===0?
                                <button type="button" className="btn btn-primary float-start"onClick={()=>this.onCreateClick()}
                                    data-bs-dismiss="modal" aria-label="Close">
                                    Create</button>:null}
                                {Id!==0?
                                <button type="button" className="btn btn-primary float-start" onClick={()=>this.onEditClick(Id)}
                                    data-bs-dismiss="modal" aria-label="Close">
                                    Update</button>:null}
                            </div>
                        </div>
                    </div>
                </div>

                <div className="modal fade" id="modalWindowForBuy" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Buy ticket</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">EventSeatId</span>
                                    <Select className="form-control" options={options} onChange={this.changeEventSeatId} />
                                </div>
                                <button type="button" className="btn btn-primary float-start"onClick={()=>this.onBuyClick(Id)}
                                    data-bs-dismiss="modal" aria-label="Close">
                                    Buy
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}