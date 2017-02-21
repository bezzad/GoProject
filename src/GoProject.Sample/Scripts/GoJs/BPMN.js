"use strict";
/*
*  Copyright (C) 1998-2017 by Northwoods Software Corporation. All Rights Reserved.
*/

// This file holds all of the JavaScript code specific to the BPMN.html page.




// Setup all of the Diagrams and what they need.
// This is called after the page is loaded.
function init() {
    // setup the menubar
    jQuery("#menuui").menu();
    jQuery(function () {
        jQuery("#menuui").menu({ position: { my: "left top", at: "left top+30" } });
    });
    jQuery("#menuui").menu({
        icons: { submenu: "ui-icon-triangle-1-s" }
    });

    var $ = go.GraphObject.make;  // for more concise visual tree definitions

    // constants for design choices

    var gradientYellow = $(go.Brush, "Linear", { 0: "LightGoldenRodYellow", 1: "#FFFF66" });
    var gradientLightGreen = $(go.Brush, "Linear", { 0: "#E0FEE0", 1: "PaleGreen" });
    var gradientLightGray = $(go.Brush, "Linear", { 0: "White", 1: "#DADADA" });

    var activityNodeFill = $(go.Brush, "Linear", { 0: "OldLace", 1: "PapayaWhip" });
    var activityNodeStroke = "#CDAA7D";
    var activityMarkerStrokeWidth = 1.5;
    var activityNodeWidth = 120;
    var activityNodeHeight = 80;
    var activityNodeStrokeWidth = 1;
    var activityNodeStrokeWidthIsCall = 4;

    var subprocessNodeFill = activityNodeFill;
    var subprocessNodeStroke = activityNodeStroke;

    var eventNodeSize = 42;
    var eventNodeInnerSize = eventNodeSize - 6;
    var eventNodeSymbolSize = eventNodeInnerSize - 14;
    var eventEndOuterFillColor = "pink";
    var eventBackgroundColor = gradientLightGreen;
    var eventSymbolLightFill = "white";
    var eventSymbolDarkFill = "dimgray";
    var eventDimensionStrokeColor = "green";
    var eventDimensionStrokeEndColor = "red";
    var eventNodeStrokeWidthIsEnd = 4;

    var gatewayNodeSize = 80;
    var gatewayNodeSymbolSize = 45;
    var gatewayNodeFill = gradientYellow;
    var gatewayNodeStroke = "darkgoldenrod";
    var gatewayNodeSymbolStroke = "darkgoldenrod";
    var gatewayNodeSymbolFill = gradientYellow;
    var gatewayNodeSymbolStrokeWidth = 3;

    var dataFill = gradientLightGray;


    // custom figures for Shapes

    go.Shape.defineFigureGenerator("Empty", function (shape, w, h) {
        return new go.Geometry();
    });

    var annotationStr = "M 150,0L 0,0L 0,600L 150,600 M 800,0";
    var annotationGeo = go.Geometry.parse(annotationStr);
    annotationGeo.normalize();
    go.Shape.defineFigureGenerator("Annotation", function (shape, w, h) {
        var geo = annotationGeo.copy();
        // calculate how much to scale the Geometry so that it fits in w x h
        var bounds = geo.bounds;
        var scale = Math.min(w / bounds.width, h / bounds.height);
        geo.scale(scale, scale);
        return geo;
    });

    var gearStr = "F M 391,5L 419,14L 444.5,30.5L 451,120.5L 485.5,126L 522,141L 595,83L 618.5,92L 644,106.5" +
      "L 660.5,132L 670,158L 616,220L 640.5,265.5L 658.122,317.809L 753.122,322.809L 770.122,348.309L 774.622,374.309" +
      "L 769.5,402L 756.622,420.309L 659.122,428.809L 640.5,475L 616.5,519.5L 670,573.5L 663,600L 646,626.5" +
      "L 622,639L 595,645.5L 531.5,597.5L 493.192,613.462L 450,627.5L 444.5,718.5L 421.5,733L 393,740.5L 361.5,733.5" +
      "L 336.5,719L 330,627.5L 277.5,611.5L 227.5,584.167L 156.5,646L 124.5,641L 102,626.5L 82,602.5L 78.5,572.5" +
      "L 148.167,500.833L 133.5,466.833L 122,432.5L 26.5,421L 11,400.5L 5,373.5L 12,347.5L 26.5,324L 123.5,317.5" +
      "L 136.833,274.167L 154,241L 75.5,152.5L 85.5,128.5L 103,105.5L 128.5,88.5001L 154.872,82.4758L 237,155" +
      "L 280.5,132L 330,121L 336,30L 361,15L 391,5 Z M 398.201,232L 510.201,275L 556.201,385L 505.201,491L 399.201,537" +
      "L 284.201,489L 242.201,385L 282.201,273L 398.201,232 Z";
    var gearGeo = go.Geometry.parse(gearStr);
    gearGeo.normalize();

    go.Shape.defineFigureGenerator("BpmnTaskService", function (shape, w, h) {
        var geo = gearGeo.copy();
        // calculate how much to scale the Geometry so that it fits in w x h
        var bounds = geo.bounds;
        var scale = Math.min(w / bounds.width, h / bounds.height);
        geo.scale(scale, scale);
        // text should go in the hand
        geo.spot1 = new go.Spot(0, 0.6, 10, 0);
        geo.spot2 = new go.Spot(1, 1);
        return geo;
    });

    var handGeo = go.Geometry.parse("F1M18.13,10.06 C18.18,10.07 18.22,10.07 18.26,10.08 18.91," +
      "10.20 21.20,10.12 21.28,12.93 21.36,15.75 21.42,32.40 21.42,32.40 21.42," +
      "32.40 21.12,34.10 23.08,33.06 23.08,33.06 22.89,24.76 23.80,24.17 24.72," +
      "23.59 26.69,23.81 27.19,24.40 27.69,24.98 28.03,24.97 28.03,33.34 28.03," +
      "33.34 29.32,34.54 29.93,33.12 30.47,31.84 29.71,27.11 30.86,26.56 31.80," +
      "26.12 34.53,26.12 34.72,28.29 34.94,30.82 34.22,36.12 35.64,35.79 35.64," +
      "35.79 36.64,36.08 36.72,34.54 36.80,33.00 37.17,30.15 38.42,29.90 39.67," +
      "29.65 41.22,30.20 41.30,32.29 41.39,34.37 42.30,46.69 38.86,55.40 35.75," +
      "63.29 36.42,62.62 33.47,63.12 30.76,63.58 26.69,63.12 26.69,63.12 26.69," +
      "63.12 17.72,64.45 15.64,57.62 13.55,50.79 10.80,40.95 7.30,38.95 3.80," +
      "36.95 4.24,36.37 4.28,35.35 4.32,34.33 7.60,31.25 12.97,35.75 12.97," +
      "35.75 16.10,39.79 16.10,42.00 16.10,42.00 15.69,14.30 15.80,12.79 15.96," +
      "10.75 17.42,10.04 18.13,10.06z ");

    handGeo.rotate(90, 0, 0);
    handGeo.normalize();
    go.Shape.defineFigureGenerator("BpmnTaskManual", function (shape, w, h) {
        var geo = handGeo.copy();
        // calculate how much to scale the Geometry so that it fits in w x h
        var bounds = geo.bounds;
        var scale = Math.min(w / bounds.width, h / bounds.height);
        geo.scale(scale, scale);
        // guess where text should go (in the hand)
        geo.spot1 = new go.Spot(0, 0.6, 10, 0);
        geo.spot2 = new go.Spot(1, 1);
        return geo;
    });


    // define the appearance of tooltips, shared by various templates
    var tooltiptemplate =
      $(go.Adornment, go.Panel.Auto,
        $(go.Shape, "RoundedRectangle",
          {
              fill: "white", // the default fill, if there is no data-binding
              portId: "", cursor: "pointer",  // the Shape is the port, not the whole Node
              // allow all kinds of links from and to this port
              fromLinkable: true, fromLinkableSelfNode: true, fromLinkableDuplicates: true,
              toLinkable: true, toLinkableSelfNode: true, toLinkableDuplicates: true
          }),
        $(go.TextBlock,
          {
              font: "bold 14px iransans, sans-serif",
              stroke: '#333',
              margin: 6,  // make some extra space for the shape around the text
              isMultiline: true,  // don't allow newlines in text
              editable: true  // allow in-place editing by user
          },
          new go.Binding("text", "", nodeInfo))
      );


    // conversion functions used by data Bindings

    function nodeActivityTaskTypeConverter(s) {
        var tasks = ["Empty",
                      "BpmnTaskMessage",
                      "BpmnTaskUser",
                      "BpmnTaskManual",   // Custom hand symbol
                      "BpmnTaskScript",
                      "BpmnTaskMessage",  // should be black on white
                      "BpmnTaskService",  // Custom gear symbol
                      "InternalStorage"];

        if (s < tasks.length) return tasks[s];
        return "NotAllowed"; // error
    }

    // location of event on boundary of Activity is based on the index of the event in the boundaryEventArray
    function nodeActivityBeSpotConverter(s) {
        var x = 10 + (eventNodeSize / 2);
        if (s === 0) return new go.Spot(0, 1, x, 0);    // bottom left
        if (s === 1) return new go.Spot(1, 1, -x, 0);   // bottom right
        if (s === 2) return new go.Spot(1, 0, -x, 0);   // top right
        return new go.Spot(1, 0, -x - (s - 2) * eventNodeSize, 0);    // top ... right-to-left-ish spread
    }

    function nodeActivityTaskTypeColorConverter(s) {
        return (s === 5) ? "dimgray" : "white";
    }

    function nodeEventTypeConverter(s) {  // order here from BPMN 2.0 poster
        var tasks = ["NotAllowed",
                      "Empty",
                      "BpmnTaskMessage",
                      "BpmnEventTimer",
                      "BpmnEventEscalation",
                      "BpmnEventConditional",
                      "Arrow",
                      "BpmnEventError",
                      "ThinX",
                      "BpmnActivityCompensation",
                      "Triangle",
                      "Pentagon",
                      "ThickCross",
                      "Circle"];
        if (s < tasks.length) return tasks[s];
        return "NotAllowed"; // error
    }

    function nodeEventDimensionStrokeColorConverter(s) {
        if (s === 8) return eventDimensionStrokeEndColor;
        return eventDimensionStrokeColor;
    }

    function nodeEventDimensionSymbolFillConverter(s) {
        if (s <= 6) return eventSymbolLightFill;
        return eventSymbolDarkFill;
    }


    //------------------------------------------  Activity Node Boundary Events   ----------------------------------------------

    var boundaryEventMenu =  // context menu for each boundaryEvent on Activity node
    $(go.Adornment, "Vertical",
      $("ContextMenuButton",
        $(go.TextBlock, "Remove event"),
        // in the click event handler, the obj.part is the Adornment; its adornedObject is the port
        { click: function (e, obj) { removeActivityNodeBoundaryEvent(obj.part.adornedObject); } })
     );

    // removing a boundary event doesn't not reposition other BE circles on the node
    // just reassigning alignmentIndex in remaining BE would do that.
    function removeActivityNodeBoundaryEvent(obj) {
        myDiagram.startTransaction("removeBoundaryEvent");
        var pid = obj.portId;
        var arr = obj.panel.itemArray;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].portId === pid) {
                myDiagram.model.removeArrayItem(arr, i);
                break;
            }
        }
        myDiagram.commitTransaction("removeBoundaryEvent");
    }

    var boundaryEventItemTemplate =
      $(go.Panel, "Spot",
         {
             contextMenu: boundaryEventMenu,
             alignmentFocus: go.Spot.Center,
             fromLinkable: true, toLinkable: false, cursor: "pointer", fromSpot: go.Spot.Bottom,
             fromMaxLinks: 1, toMaxLinks: 0
         },
         new go.Binding("portId", "portId"),
         new go.Binding("alignment", "alignmentIndex", nodeActivityBeSpotConverter),
        $(go.Shape, "Circle",
          { desiredSize: new go.Size(eventNodeSize, eventNodeSize) },
          new go.Binding("strokeDashArray", "eventDimension", function (s) { return (s === 6) ? [4, 2] : null; }),
          new go.Binding("fromSpot", "alignmentIndex",
            function (s) {
                //  nodeActivityBEFromSpotConverter, 0 & 1 go on bottom, all others on top of activity
                if (s < 2) return go.Spot.Bottom;
                return go.Spot.Top;
            }),
          new go.Binding("fill", "color")),
        $(go.Shape, "Circle",
           {
               alignment: go.Spot.Center,
               desiredSize: new go.Size(eventNodeInnerSize, eventNodeInnerSize), fill: null
           },
           new go.Binding("strokeDashArray", "eventDimension", function (s) { return (s === 6) ? [4, 2] : null; })
         ),
        $(go.Shape, "NotAllowed",
            {
                alignment: go.Spot.Center,
                desiredSize: new go.Size(eventNodeSymbolSize, eventNodeSymbolSize), fill: "white"
            },
              new go.Binding("figure", "eventType", nodeEventTypeConverter)
          )
      );

    //------------------------------------------  Activity Node contextMenu   ----------------------------------------------

    //var activityNodeMenu =
    //     $(go.Adornment, "Vertical",
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add Email Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 2, 5); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add Timer Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 3, 5); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add Escalation Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 4, 5); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add Error Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 7, 5); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add Signal Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 10, 5); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Add N-I Escalation Event", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 4, 6); } }),
    //       $("ContextMenuButton",
    //           $(go.TextBlock, "Rename", { margin: 3, font: "10px iransans, sans-serif" }),
    //           { click: function (e, obj) { rename(myDiagram, obj); } }));

    var activityNodeMenu =
     $(go.Adornment, "Vertical",
       $("ContextMenuButton",
           $(go.TextBlock, "اضافه کردن رخداد ایمیل", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 2, 5); } }),
       $("ContextMenuButton",
           $(go.TextBlock, "اضافه کردن رخداد زماندار", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 3, 5); } }),
       $("ContextMenuButton",
           $(go.TextBlock, "اضافه کردن رخداد تشدید", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 4, 5); } }),
       $("ContextMenuButton",
           $(go.TextBlock, "اضافه کردن رخداد خطا", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 7, 5); } }),
       $("ContextMenuButton",
           $(go.TextBlock, "اضافه کردن رخداد سیگنال", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { addActivityNodeBoundaryEvent(myDiagram, 10, 5); } }),
       $("ContextMenuButton",
           $(go.TextBlock, "تغییر نام", { margin: 3, font: "10px iransans, sans-serif" }),
           { click: function (e, obj) { rename(myDiagram, obj); } }));


    // sub-process,  loop, parallel, sequential, ad doc and compensation markers in horizontal array
    function makeSubButton(sub) {
        if (sub)
            return [$("SubGraphExpanderButton"),
                    { margin: 2, visible: false },
                         new go.Binding("visible", "isSubProcess")];
        return [];
    }

    // sub-process,  loop, parallel, sequential, ad doc and compensation markers in horizontal array
    function makeMarkerPanel(sub, scale) {
        return $(go.Panel, "Horizontal",
                { alignment: go.Spot.MiddleBottom, alignmentFocus: go.Spot.MiddleBottom },
                $(go.Shape, "BpmnActivityLoop",
                  { width: 12 / scale, height: 12 / scale, margin: 2, visible: false, strokeWidth: activityMarkerStrokeWidth },
                  new go.Binding("visible", "isLoop")),
                $(go.Shape, "BpmnActivityParallel",
                  { width: 12 / scale, height: 12 / scale, margin: 2, visible: false, strokeWidth: activityMarkerStrokeWidth },
                  new go.Binding("visible", "isParallel")),
                $(go.Shape, "BpmnActivitySequential",
                  { width: 12 / scale, height: 12 / scale, margin: 2, visible: false, strokeWidth: activityMarkerStrokeWidth },
                  new go.Binding("visible", "isSequential")),
                $(go.Shape, "BpmnActivityAdHoc",
                  { width: 12 / scale, height: 12 / scale, margin: 2, visible: false, strokeWidth: activityMarkerStrokeWidth },
                  new go.Binding("visible", "isAdHoc")),
                $(go.Shape, "BpmnActivityCompensation",
                  { width: 12 / scale, height: 12 / scale, margin: 2, visible: false, strokeWidth: activityMarkerStrokeWidth, fill: null },
                  new go.Binding("visible", "isCompensation")),
                makeSubButton(sub)
              ); // end activity markers horizontal panel
    }

    var activityNodeTemplate =
    $(go.Node, "Spot",
       {
           locationObjectName: "SHAPE", locationSpot: go.Spot.Center,
           resizable: true, resizeObjectName: "PANEL",
           toolTip: tooltiptemplate,
           selectionAdorned: false,  // use a Binding on the Shape.stroke to show selection
           contextMenu: activityNodeMenu,
           itemTemplate: boundaryEventItemTemplate
       },
       new go.Binding("itemArray", "boundaryEventArray"),
       new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
       // move a selected part into the Foreground layer, so it isn"t obscured by any non-selected parts
       new go.Binding("layerName", "isSelected", function (s) { return s ? "Foreground" : ""; }).ofObject(),
      $(go.Panel, "Auto",
        {
            name: "PANEL",
            minSize: new go.Size(activityNodeWidth, activityNodeHeight),
            desiredSize: new go.Size(activityNodeWidth, activityNodeHeight)
        },
        new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
         $(go.Panel, "Spot",
          $(go.Shape, "RoundedRectangle",  // the outside rounded rectangle
            {
                name: "SHAPE",
                fill: activityNodeFill, stroke: activityNodeStroke,
                parameter1: 10, // corner size
                portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
                fromSpot: go.Spot.RightSide, toSpot: go.Spot.LeftSide
            },
            new go.Binding("fill", "color"),
            new go.Binding("strokeWidth", "isCall",
                 function (s) { return s ? activityNodeStrokeWidthIsCall : activityNodeStrokeWidth; })
           ),
          // task icon
          $(go.Shape, "BpmnTaskScript",    // will be None, Script, Manual, Service, etc via converter
            {
                alignment: new go.Spot(0, 0, 5, 5), alignmentFocus: go.Spot.TopLeft,
                width: 22, height: 22
            },
            new go.Binding("fill", "taskType", nodeActivityTaskTypeColorConverter),
            new go.Binding("figure", "taskType", nodeActivityTaskTypeConverter)
            ), // end Task Icon
          makeMarkerPanel(false, 1) // sub-process,  loop, parallel, sequential, ad doc and compensation markers
          ),  // end main body rectangles spot panel
        $(go.TextBlock,  // the center text
          {
              font: "bold 11px iransans, sans-serif", // Task shape font
              alignment: go.Spot.Center, textAlign: "center", margin: 12,
              editable: true
          },
          new go.Binding("text").makeTwoWay())
        )  // end Auto Panel
      );  // end go.Node, which is a Spot Panel with bound itemArray

    // ------------------------------- template for Activity / Task node in Palette  -------------------------------

    var palscale = 2;
    var activityNodeTemplateForPalette =
    $(go.Node, "Vertical",
       {
           locationObjectName: "SHAPE",
           locationSpot: go.Spot.Center,
           selectionAdorned: false
       },
      new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
      $(go.Panel, "Spot",
        {
            name: "PANEL",
            desiredSize: new go.Size(activityNodeWidth / palscale, activityNodeHeight / palscale)
        },
        $(go.Shape, "RoundedRectangle",  // the outside rounded rectangle
          {
              name: "SHAPE",
              fill: activityNodeFill, stroke: activityNodeStroke,
              parameter1: 10 / palscale  // corner size (default 10)
          },
          new go.Binding("strokeWidth", "isCall",
              function (s) { return s ? activityNodeStrokeWidthIsCall : activityNodeStrokeWidth; })),
        $(go.Shape, "RoundedRectangle",  // the inner "Transaction" rounded rectangle
          {
              margin: 3,
              stretch: go.GraphObject.Fill,
              stroke: activityNodeStroke,
              parameter1: 8 / palscale, fill: null, visible: false
          },
          new go.Binding("visible", "isTransaction")),
        // task icon
        $(go.Shape, "BpmnTaskScript",    // will be None, Script, Manual, Service, etc via converter
          {
              alignment: new go.Spot(0, 0, 5, 5), alignmentFocus: go.Spot.TopLeft,
              width: 22 / palscale, height: 22 / palscale
          },
          new go.Binding("fill", "taskType", nodeActivityTaskTypeColorConverter),
          new go.Binding("figure", "taskType", nodeActivityTaskTypeConverter)),
        makeMarkerPanel(false, palscale) // sub-process,  loop, parallel, sequential, ad doc and compensation markers
      ), // End Spot panel
      $(go.TextBlock,  // the center text
          { alignment: go.Spot.Center, textAlign: "center", margin: 2, font: "11px iransans, sans-serif" },
          new go.Binding("text"))
      );  // End Node

    var subProcessGroupTemplateForPalette =
    $(go.Group, "Vertical",
       {
           locationObjectName: "SHAPE",
           locationSpot: go.Spot.Center,
           isSubGraphExpanded: false,
           selectionAdorned: false
       },
      $(go.Panel, "Spot",
        {
            name: "PANEL",
            desiredSize: new go.Size(activityNodeWidth / palscale, activityNodeHeight / palscale)
        },
        $(go.Shape, "RoundedRectangle",  // the outside rounded rectangle
              {
                  name: "SHAPE",
                  fill: activityNodeFill, stroke: activityNodeStroke,
                  parameter1: 10 / palscale  // corner size (default 10)
              },
                new go.Binding("strokeWidth", "isCall", function (s) { return s ? activityNodeStrokeWidthIsCall : activityNodeStrokeWidth; })
            ),
        $(go.Shape, "RoundedRectangle",  // the inner "Transaction" rounded rectangle
          {
              margin: 3,
              stretch: go.GraphObject.Fill,
              stroke: activityNodeStroke,
              parameter1: 8 / palscale, fill: null, visible: false
          },
          new go.Binding("visible", "isTransaction")),
          makeMarkerPanel(true, palscale) // sub-process,  loop, parallel, sequential, ad doc and compensation markers
        ), // end main body rectangles spot panel
        $(go.TextBlock,  // the center text
          { alignment: go.Spot.Center, textAlign: "center", margin: 2, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text"))
      );  // end go.Group

    //------------------------------------------  Event Node Template  ----------------------------------------------

    var eventNodeTemplate =
      $(go.Node, "Vertical",
        {
            locationObjectName: "SHAPE",
            locationSpot: go.Spot.Center,
            toolTip: tooltiptemplate
        },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        // move a selected part into the Foreground layer, so it isn't obscured by any non-selected parts
        new go.Binding("layerName", "isSelected", function (s) { return s ? "Foreground" : ""; }).ofObject(),
        // can be resided according to the user's desires
        { resizable: false, resizeObjectName: "SHAPE" },
          $(go.Panel, "Spot",
            $(go.Shape, "Circle",  // Outer circle
              {
                  strokeWidth: 1,
                  name: "SHAPE",
                  desiredSize: new go.Size(eventNodeSize, eventNodeSize),
                  portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
                  fromSpot: go.Spot.RightSide, toSpot: go.Spot.LeftSide
              },
              // allows the color to be determined by the node data
              new go.Binding("fill", "eventDimension", function (s) { return (s === 8) ? eventEndOuterFillColor : eventBackgroundColor; }),
              new go.Binding("strokeWidth", "eventDimension", function (s) { return s === 8 ? eventNodeStrokeWidthIsEnd : 1; }),
              new go.Binding("stroke", "eventDimension", nodeEventDimensionStrokeColorConverter),
              new go.Binding("strokeDashArray", "eventDimension", function (s) { return (s === 3 || s === 6) ? [4, 2] : null; }),
              new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify)
            ),  // end main shape
            $(go.Shape, "Circle",  // Inner circle
                { alignment: go.Spot.Center, desiredSize: new go.Size(eventNodeInnerSize, eventNodeInnerSize), fill: null },
                new go.Binding("stroke", "eventDimension", nodeEventDimensionStrokeColorConverter),
                new go.Binding("strokeDashArray", "eventDimension", function (s) { return (s === 3 || s === 6) ? [4, 2] : null; }), // dashes for non-interrupting
                new go.Binding("visible", "eventDimension", function (s) { return s > 3 && s <= 7; }) // inner  only visible for 4 thru 7
              ),
  $(go.Shape, "NotAllowed",
                { alignment: go.Spot.Center, desiredSize: new go.Size(eventNodeSymbolSize, eventNodeSymbolSize), stroke: "black" },
                  new go.Binding("figure", "eventType", nodeEventTypeConverter),
                  new go.Binding("fill", "eventDimension", nodeEventDimensionSymbolFillConverter)
              )
          ),  // end Auto Panel
          $(go.TextBlock,
            { alignment: go.Spot.Center, textAlign: "center", margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
            new go.Binding("text").makeTwoWay())

        ); // end go.Node Vertical

    //------------------------------------------  Gateway Node Template   ----------------------------------------------

    function nodeGatewaySymbolTypeConverter(s) {
        var tasks = [
            "NotAllowed",
            "ThinCross", // 1 - Parallel
            "Circle", // 2 - Inclusive
            "AsteriskLine", // 3 - Complex
            "ThinX", // 4 - Exclusive  (exclusive can also be no symbol, just bind to visible=false for no symbol)
            "Pentagon", // 5 - double cicle event based gateway
            "Pentagon", // 6 - exclusive event gateway to start a process (single circle)
            "ThickCross"
        ];     // 7 - parallel event gateway to start a process (single circle)
        if (s < tasks.length) return tasks[s];
        return "NotAllowed"; // error
    }

    // tweak the size of some of the gateway icons
    function nodeGatewaySymbolSizeConverter(s) {
        var size = new go.Size(gatewayNodeSymbolSize, gatewayNodeSymbolSize);
        if (s === 4) {
            size.width = size.width / 4 * 3;
            size.height = size.height / 4 * 3;
        }
        else if (s > 4) {
            size.width = size.width / 1.6;
            size.height = size.height / 1.6;
        }
        return size;
    }
    function nodePalGatewaySymbolSizeConverter(s) {
        var size = nodeGatewaySymbolSizeConverter(s);
        size.width = size.width / 2;
        size.height = size.height / 2;
        return size;
    }

    var gatewayNodeTemplate =
      $(go.Node, "Vertical",
        {
            locationObjectName: "SHAPE",
            locationSpot: go.Spot.Center,
            toolTip: tooltiptemplate
        },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        // move a selected part into the Foreground layer, so it isn't obscured by any non-selected parts
        new go.Binding("layerName", "isSelected", function (s) { return s ? "Foreground" : ""; }).ofObject(),
        // can be resided according to the user's desires
        { resizable: false, resizeObjectName: "SHAPE" },
          $(go.Panel, "Spot",
            $(go.Shape, "Diamond",
              {
                  strokeWidth: 1, fill: gatewayNodeFill, stroke: gatewayNodeStroke,
                  name: "SHAPE",
                  desiredSize: new go.Size(gatewayNodeSize, gatewayNodeSize),
                  portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
                  fromSpot: go.Spot.NotLeftSide, toSpot: go.Spot.MiddleLeft
              },
              new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify)),  // end main shape
            $(go.Shape, "NotAllowed",
                { alignment: go.Spot.Center, stroke: gatewayNodeSymbolStroke, fill: gatewayNodeSymbolFill },
                  new go.Binding("figure", "gatewayType", nodeGatewaySymbolTypeConverter),
                  //new go.Binding("visible", "gatewayType", function(s) { return s !== 4; }),   // comment out if you want exclusive gateway to be X instead of blank.
                  new go.Binding("strokeWidth", "gatewayType", function (s) { return (s <= 4) ? gatewayNodeSymbolStrokeWidth : 1; }),
                  new go.Binding("desiredSize", "gatewayType", nodeGatewaySymbolSizeConverter)),
            // the next 2 circles only show up for event gateway
            $(go.Shape, "Circle",  // Outer circle
              {
                  strokeWidth: 1, stroke: gatewayNodeSymbolStroke, fill: null, desiredSize: new go.Size(eventNodeSize, eventNodeSize)
              },
              new go.Binding("visible", "gatewayType", function (s) { return s >= 5; }) // only visible for > 5
            ),  // end main shape
            $(go.Shape, "Circle",  // Inner circle
                {
                    alignment: go.Spot.Center, stroke: gatewayNodeSymbolStroke,
                    desiredSize: new go.Size(eventNodeInnerSize, eventNodeInnerSize),
                    fill: null
                },
                new go.Binding("visible", "gatewayType", function (s) { return s === 5; }) // inner  only visible for == 5
              )
           ),
          $(go.TextBlock,
            { alignment: go.Spot.Center, textAlign: "center", margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
            new go.Binding("text").makeTwoWay())
        ); // end go.Node Vertical

    //--------------------------------------------------------------------------------------------------------------

    var gatewayNodeTemplateForPalette =
      $(go.Node, "Vertical",
        {
            toolTip: tooltiptemplate,
            resizable: false,
            locationObjectName: "SHAPE",
            locationSpot: go.Spot.Center,
            resizeObjectName: "SHAPE"
        },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        $(go.Panel, "Spot",
          $(go.Shape, "Diamond",
            {
                strokeWidth: 1, fill: gatewayNodeFill, stroke: gatewayNodeStroke, name: "SHAPE",
                desiredSize: new go.Size(gatewayNodeSize / 2, gatewayNodeSize / 2)
            }),
          $(go.Shape, "NotAllowed",
              { alignment: go.Spot.Center, stroke: gatewayNodeSymbolStroke, strokeWidth: gatewayNodeSymbolStrokeWidth, fill: gatewayNodeSymbolFill },
                new go.Binding("figure", "gatewayType", nodeGatewaySymbolTypeConverter),
                //new go.Binding("visible", "gatewayType", function(s) { return s !== 4; }),   // comment out if you want exclusive gateway to be X instead of blank.
                new go.Binding("strokeWidth", "gatewayType", function (s) { return (s <= 4) ? gatewayNodeSymbolStrokeWidth : 1; }),
                new go.Binding("desiredSize", "gatewayType", nodePalGatewaySymbolSizeConverter)),
            // the next 2 circles only show up for event gateway
            $(go.Shape, "Circle",  // Outer circle
              {
                  strokeWidth: 1, stroke: gatewayNodeSymbolStroke, fill: null, desiredSize: new go.Size(eventNodeSize / 2, eventNodeSize / 2)
              },
              //new go.Binding("desiredSize", "gatewayType", new go.Size(EventNodeSize/2, EventNodeSize/2)),
              new go.Binding("visible", "gatewayType", function (s) { return s >= 5; }) // only visible for > 5
            ),  // end main shape
            $(go.Shape, "Circle",  // Inner circle
                {
                    alignment: go.Spot.Center, stroke: gatewayNodeSymbolStroke,
                    desiredSize: new go.Size(eventNodeInnerSize / 2, eventNodeInnerSize / 2),
                    fill: null
                },
                new go.Binding("visible", "gatewayType", function (s) { return s === 5; }) // inner  only visible for == 5
              )),

        $(go.TextBlock,
          { alignment: go.Spot.Center, textAlign: "center", margin: 5, editable: false, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text"))
      );

    //--------------------------------------------------------------------------------------------------------------

    var annotationNodeTemplate =
      $(go.Node, "Auto",
        {
            background: gradientLightGray,
            locationSpot: go.Spot.Center
            //,click: doMouseOver  // this event handler is defined below
        },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        $(go.Shape, "Annotation", // A left bracket shape
          { portId: "", fromLinkable: true, cursor: "pointer", fromSpot: go.Spot.Left, strokeWidth: 2, stroke: "gray" }),
        $(go.TextBlock,
          { margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text").makeTwoWay())
      );

    var dataObjectNodeTemplate =
      $(go.Node, "Vertical",
        { locationObjectName: "SHAPE", locationSpot: go.Spot.Center },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        $(go.Shape, "File",
          {
              name: "SHAPE", portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
              fill: dataFill, desiredSize: new go.Size(eventNodeSize * 0.8, eventNodeSize)
          }),
        $(go.TextBlock,
          {
              margin: 5,
              editable: true
          },
            new go.Binding("text").makeTwoWay())
      );

    var dataStoreNodeTemplate =
      $(go.Node, "Vertical",
        { locationObjectName: "SHAPE", locationSpot: go.Spot.Center },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        $(go.Shape, "Database",
          {
              name: "SHAPE", portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
              fill: dataFill, desiredSize: new go.Size(eventNodeSize, eventNodeSize)
          }),
        $(go.TextBlock,
          { margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text").makeTwoWay())
      );

    //------------------------------------------  private process Node Template Map   ----------------------------------------------

    var privateProcessNodeTemplate =
      $(go.Node, "Auto",
        { layerName: "Background", resizable: true, resizeObjectName: "LANE" },
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        $(go.Shape, "Rectangle",
          { fill: null }),
        $(go.Panel, "Table",     // table with 2 cells to hold header and lane
          {
              desiredSize: new go.Size(activityNodeWidth * 6, activityNodeHeight),
              background: dataFill, name: "LANE", minSize: new go.Size(activityNodeWidth, activityNodeHeight * 0.667)
          },
          new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
          $(go.TextBlock,
            {
                row: 0, column: 0,
                angle: 270, margin: 5,
                editable: true, textAlign: "center",
                font: "bold 11px iransans, sans-serif"
            },
            new go.Binding("text").makeTwoWay()),
          $(go.RowColumnDefinition, { column: 1, separatorStrokeWidth: 1, separatorStroke: "black" }),
          $(go.Shape, "Rectangle",
            {
                row: 0, column: 1,
                stroke: null, fill: "transparent",
                portId: "", fromLinkable: true, toLinkable: true,
                fromSpot: go.Spot.TopBottomSides, toSpot: go.Spot.TopBottomSides,
                cursor: "pointer", stretch: go.GraphObject.Fill
            })
         )
      );

    var privateProcessNodeTemplateForPalette =
      $(go.Node, "Vertical",
        { locationSpot: go.Spot.Center },
        $(go.Shape, "Process",
          { fill: dataFill, desiredSize: new go.Size(gatewayNodeSize / 2, gatewayNodeSize / 4) }),
        $(go.TextBlock,
          { margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text"))
      );

    var poolTemplateForPalette =
      $(go.Group, "Vertical",
        {
            locationSpot: go.Spot.Center,
            computesBoundsIncludingLinks: false,
            isSubGraphExpanded: false
        },
        $(go.Shape, "Process",
          { fill: "white", desiredSize: new go.Size(gatewayNodeSize / 2, gatewayNodeSize / 4) }),
        $(go.Shape, "Process",
          { fill: "white", desiredSize: new go.Size(gatewayNodeSize / 2, gatewayNodeSize / 4) }),
        $(go.TextBlock,
          { margin: 5, editable: true, font: "bold 11px iransans, sans-serif" },
          new go.Binding("text"))
      );

    var swimLanesGroupTemplateForPalette =
      $(go.Group, "Vertical"); // empty in the palette

    var subProcessGroupTemplate =
      $(go.Group, "Spot",
        {
            locationSpot: go.Spot.Center,
            locationObjectName: "PH",
            resizable: true, resizeObjectName: "PH",
            //locationSpot: go.Spot.Center,
            isSubGraphExpanded: false,
            memberValidation: function (group, part) {
                return !(part instanceof go.Group) ||
                       (part.category !== "Pool" && part.category !== "Lane");
            },
            mouseDrop: function (e, grp) {
                var ok = grp.addMembers(grp.diagram.selection, true);
                if (!ok) grp.diagram.currentTool.doCancel();
            },
            contextMenu: activityNodeMenu,
            itemTemplate: boundaryEventItemTemplate
        },
        new go.Binding("itemArray", "boundaryEventArray"),
        new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
        // move a selected part into the Foreground layer, so it isn't obscured by any non-selected parts
        // new go.Binding("layerName", "isSelected", function (s) { return s ? "Foreground" : ""; }).ofObject(),
        $(go.Panel, "Auto",
          $(go.Shape, "RoundedRectangle",
              {
                  name: "PH", fill: subprocessNodeFill, stroke: subprocessNodeStroke,
                  minSize: new go.Size(activityNodeWidth, activityNodeHeight),
                  portId: "", fromLinkable: true, toLinkable: true, cursor: "pointer",
                  fromSpot: go.Spot.RightSide, toSpot: go.Spot.LeftSide
              },
              new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
              new go.Binding("strokeWidth", "isCall", function (s) { return s ? activityNodeStrokeWidthIsCall : activityNodeStrokeWidth; })
             ),
            $(go.Panel, "Vertical",
              { defaultAlignment: go.Spot.Left },
              $(go.TextBlock,  // label
                { margin: 3, editable: true, font: "bold 11px iransans, sans-serif" },
                new go.Binding("text", "text").makeTwoWay(),
                new go.Binding("alignment", "isSubGraphExpanded", function (s) { return s ? go.Spot.TopLeft : go.Spot.Center; })),
              // create a placeholder to represent the area where the contents of the group are
              $(go.Placeholder,
                { padding: new go.Margin(5, 5) }),
              makeMarkerPanel(true, 1)  // sub-process,  loop, parallel, sequential, ad doc and compensation markers
            )  // end Vertical Panel
          )
        );  // end Group

    function groupStyle() {  // common settings for both Lane and Pool Groups
        return [
          {
              layerName: "Background",  // all pools and lanes are always behind all nodes and links
              background: "transparent",  // can grab anywhere in bounds
              movable: true, // allows users to re-order by dragging
              copyable: false,  // can't copy lanes or pools
              avoidable: false  // don't impede AvoidsNodes routed Links
          },
          new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify)
        ];
    }

    // hide links between lanes when either lane is collapsed
    function updateCrossLaneLinks(group) {
        group.findExternalLinksConnected().each(function (l) {
            l.visible = (l.fromNode.isVisible() && l.toNode.isVisible());
        });
    }

    var laneEventMenu =  // context menu for each lane
        $(go.Adornment, "Vertical",
          $("ContextMenuButton",
            $(go.TextBlock, "ایجاد بخش"),
            // in the click event handler, the obj.part is the Adornment; its adornedObject is the port
              { click: function (e, obj) { addLaneEvent(myDiagram, obj.part.adornedObject); } })
         );



    var swimLanesGroupTemplate =
    $(go.Group, "Spot", groupStyle(),
      {
          name: "Lane",
          contextMenu: laneEventMenu,
          minLocation: new go.Point(NaN, -Infinity),  // only allow vertical movement
          maxLocation: new go.Point(NaN, Infinity),
          selectionObjectName: "SHAPE",  // selecting a lane causes the body of the lane to be highlit, not the label
          resizable: true, resizeObjectName: "SHAPE",  // the custom resizeAdornmentTemplate only permits two kinds of resizing
          layout: $(go.LayeredDigraphLayout,  // automatically lay out the lane's subgraph
                    {
                        isInitial: false,  // don't even do initial layout
                        isOngoing: false,  // don't invalidate layout when nodes or links are added or removed
                        direction: 0,
                        columnSpacing: 10,
                        layeringOption: go.LayeredDigraphLayout.LayerLongestPathSource
                    }),
          computesBoundsAfterDrag: true,  // needed to prevent recomputing Group.placeholder bounds too soon
          computesBoundsIncludingLinks: false,  // to reduce occurrences of links going briefly outside the lane
          computesBoundsIncludingLocation: true,  // to support empty space at top-left corner of lane
          handlesDragDropForMembers: true,  // don't need to define handlers on member Nodes and Links
          mouseDrop: function (e, grp) {  // dropping a copy of some Nodes and Links onto this Group adds them to this Group
              // don't allow drag-and-dropping a mix of regular Nodes and Groups
              if (!e.diagram.selection.any(function (n) { return (n instanceof go.Group && n.category !== "subprocess") || n.category === "privateProcess"; })) {
                  var ok = grp.addMembers(grp.diagram.selection, true);
                  if (ok) {
                      updateCrossLaneLinks(grp);
                      relayoutDiagram(myDiagram);
                  } else {
                      grp.diagram.currentTool.doCancel();
                  }
              }
          },
          subGraphExpandedChanged: function (grp) {
              var shp = grp.resizeObject;
              if (grp.diagram.undoManager.isUndoingRedoing) return;
              if (grp.isSubGraphExpanded) {
                  shp.height = grp._savedBreadth;
              } else {
                  grp._savedBreadth = shp.height;
                  shp.height = NaN;
              }
              updateCrossLaneLinks(grp);
          }
      },
      //new go.Binding("isSubGraphExpanded", "expanded").makeTwoWay(),

      $(go.Shape, "Rectangle",  // this is the resized object
          { name: "SHAPE", fill: "white", stroke: null },  // need stroke null here or you gray out some of pool border.
        new go.Binding("fill", "color"),
        new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify)),

     // the lane header consisting of a Shape and a TextBlock
        $(go.Panel, "Horizontal",
        {
            name: "HEADER",
            angle: 270,  // maybe rotate the header to read sideways going up
            alignment: go.Spot.LeftCenter, alignmentFocus: go.Spot.LeftCenter
        },
          $(go.TextBlock,  // the lane label
            { editable: true, margin: new go.Margin(2, 0, 0, 8), font: "bold 11px iransans, sans-serif" },
            new go.Binding("visible", "isSubGraphExpanded").ofObject(),
            new go.Binding("text", "text").makeTwoWay()),
          $("SubGraphExpanderButton", { margin: 4, angle: -270 })  // but this remains always visible!
         ),  // end Horizontal Panel
        $(go.Placeholder,
          { padding: 12, alignment: go.Spot.TopLeft, alignmentFocus: go.Spot.TopLeft }),
      $(go.Panel, "Horizontal", { alignment: go.Spot.TopLeft, alignmentFocus: go.Spot.TopLeft },
        $(go.TextBlock,  // this TextBlock is only seen when the swimlane is collapsed
          {
              name: "LABEL",
              editable: true, visible: false,
              angle: 0, margin: new go.Margin(6, 0, 0, 20),
              font: "bold 11px iransans, sans-serif"
          },
            new go.Binding("visible", "isSubGraphExpanded", function (e) { return !e; }).ofObject(),
          new go.Binding("text", "text").makeTwoWay())
       )
    );  // end swimLanesGroupTemplate

    // define a custom resize adornment that has two resize handles if the group is expanded
    //myDiagram.groupTemplate.resizeAdornmentTemplate =
    swimLanesGroupTemplate.resizeAdornmentTemplate =
      $(go.Adornment, "Spot",
        $(go.Placeholder),
        $(go.Shape,  // for changing the length of a lane
          {
              alignment: go.Spot.Right,
              desiredSize: new go.Size(7, 50),
              fill: "lightblue", stroke: "dodgerblue",
              cursor: "col-resize"
          },
          new go.Binding("visible", "", function (ad) { return ad.adornedPart.isSubGraphExpanded; }).ofObject()),
        $(go.Shape,  // for changing the breadth of a lane
          {
              alignment: go.Spot.Bottom,
              desiredSize: new go.Size(50, 7),
              fill: "lightblue", stroke: "dodgerblue",
              cursor: "row-resize"
          },
          new go.Binding("visible", "", function (ad) { return ad.adornedPart.isSubGraphExpanded; }).ofObject())
      );

    var poolGroupTemplate =
       $(go.Group, "Auto", groupStyle(),
         {
             computesBoundsIncludingLinks: false,
             // use a simple layout that ignores links to stack the "lane" Groups on top of each other
             layout: $(PoolLayout, { spacing: new go.Size(0, 0) })  // no space between lanes
         },
         new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
         $(go.Shape,
           { fill: "white" },
           new go.Binding("fill", "color")),
         $(go.Panel, "Table",
           { defaultColumnSeparatorStroke: "black" },
           $(go.Panel, "Horizontal",
             { column: 0, angle: 270 },
             $(go.TextBlock,
               { editable: true, margin: new go.Margin(5, 0, 5, 0), font: "bold 11px iransans, sans-serif" },  // margin matches private process (black box pool)
               new go.Binding("text").makeTwoWay())
           ),
           $(go.Placeholder,
             { background: "darkgray", column: 1 })
         )
       ); // end poolGroupTemplate

    //------------------------------------------  Template Maps  ----------------------------------------------

    // create the nodeTemplateMap, holding main view node templates:
    var nodeTemplateMap = new go.Map("string", go.Node);
    // for each of the node categories, specify which template to use
    nodeTemplateMap.add("activity", activityNodeTemplate);
    nodeTemplateMap.add("event", eventNodeTemplate);
    nodeTemplateMap.add("gateway", gatewayNodeTemplate);
    nodeTemplateMap.add("annotation", annotationNodeTemplate);
    nodeTemplateMap.add("dataobject", dataObjectNodeTemplate);
    nodeTemplateMap.add("datastore", dataStoreNodeTemplate);
    nodeTemplateMap.add("privateProcess", privateProcessNodeTemplate);
    // for the default category, "", use the same template that Diagrams use by default
    // this just shows the key value as a simple TextBlock

    var groupTemplateMap = new go.Map("string", go.Group);
    groupTemplateMap.add("subprocess", subProcessGroupTemplate);
    groupTemplateMap.add("Lane", swimLanesGroupTemplate);
    groupTemplateMap.add("Pool", poolGroupTemplate);

    // create the nodeTemplateMap, holding special palette "mini" node templates:
    var palNodeTemplateMap = new go.Map("string", go.Node);
    palNodeTemplateMap.add("activity", activityNodeTemplateForPalette);
    palNodeTemplateMap.add("event", eventNodeTemplate);
    palNodeTemplateMap.add("gateway", gatewayNodeTemplateForPalette);
    palNodeTemplateMap.add("annotation", annotationNodeTemplate);
    palNodeTemplateMap.add("dataobject", dataObjectNodeTemplate);
    palNodeTemplateMap.add("datastore", dataStoreNodeTemplate);
    palNodeTemplateMap.add("privateProcess", privateProcessNodeTemplateForPalette);

    var palGroupTemplateMap = new go.Map("string", go.Group);
    palGroupTemplateMap.add("subprocess", subProcessGroupTemplateForPalette);
    palGroupTemplateMap.add("Pool", poolTemplateForPalette);
    palGroupTemplateMap.add("Lane", swimLanesGroupTemplateForPalette);


    //------------------------------------------  Link Templates   ----------------------------------------------

    var sequenceLinkTemplate =
      $(go.Link,
        {
            contextMenu:
              $(go.Adornment, "Vertical",
                $("ContextMenuButton",
                  $(go.TextBlock, "Default Flow"),
                  // in the click event handler, the obj.part is the Adornment; its adornedObject is the port
                  { click: function (e, obj) { setSequenceLinkDefaultFlow(obj.part.adornedObject); } }),
                $("ContextMenuButton",
                  $(go.TextBlock, "Conditional Flow"),
                  // in the click event handler, the obj.part is the Adornment; its adornedObject is the port
                  { click: function (e, obj) { setSequenceLinkConditionalFlow(obj.part.adornedObject); } })
               ),
            routing: go.Link.AvoidsNodes, curve: go.Link.JumpGap, corner: 10,
            //fromSpot: go.Spot.RightSide, toSpot: go.Spot.LeftSide,
            reshapable: true, relinkableFrom: true, relinkableTo: true, toEndSegmentLength: 20
        },
        new go.Binding("points").makeTwoWay(),
        $(go.Shape, { stroke: "black", strokeWidth: 1 }),
        $(go.Shape, { toArrow: "Triangle", scale: 1.2, fill: "black", stroke: null }),
        $(go.Shape, { fromArrow: "", scale: 1.5, stroke: "black", fill: "white" },
                    new go.Binding("fromArrow", "isDefault", function (s) {
                        if (s === null) return "";
                        return s ? "BackSlash" : "StretchedDiamond";
                    }),
                    new go.Binding("segmentOffset", "isDefault", function (s) {
                        return s ? new go.Point(5, 0) : new go.Point(0, 0);
                    })),
        $(go.TextBlock, { // this is a Link label
            name: "Label", editable: true, text: "label", segmentOffset: new go.Point(-10, -10), visible: false, font: "bold 14px iransans, sans-serif"
        },
          new go.Binding("text", "text").makeTwoWay(),
          new go.Binding("visible", "visible").makeTwoWay())
     );

    // set Default Sequence Flow (backslash From Arrow)
    function setSequenceLinkDefaultFlow(obj) {
        myDiagram.startTransaction("setSequenceLinkDefaultFlow");
        var model = myDiagram.model;
        model.setDataProperty(obj.data, "isDefault", true);
        // Set all other links from the fromNode to be isDefault=null
        obj.fromNode.findLinksOutOf().each(function (link) {
            if (link !== obj && link.data.isDefault) {
                model.setDataProperty(link.data, "isDefault", null);
            }
        });
        myDiagram.commitTransaction("setSequenceLinkDefaultFlow");
    }

    // set Conditional Sequence Flow (diamond From Arrow)
    function setSequenceLinkConditionalFlow(obj) {
        myDiagram.startTransaction("setSequenceLinkConditionalFlow");
        var model = myDiagram.model;
        model.setDataProperty(obj.data, "isDefault", false);
        myDiagram.commitTransaction("setSequenceLinkConditionalFlow");
    }

    var messageFlowLinkTemplate =
       $(PoolLink, // defined in BPMNClasses.js
         {
             routing: go.Link.Orthogonal, curve: go.Link.JumpGap, corner: 10,
             fromSpot: go.Spot.TopBottomSides, toSpot: go.Spot.TopBottomSides,
             reshapable: true, relinkableTo: true, toEndSegmentLength: 20
         },
         new go.Binding("points").makeTwoWay(),
         $(go.Shape, { stroke: "black", strokeWidth: 1, strokeDashArray: [6, 2] }),
         $(go.Shape, { toArrow: "Triangle", scale: 1, fill: "white", stroke: "black" }),
         $(go.Shape, { fromArrow: "Circle", scale: 1, visible: true, stroke: "black", fill: "white" }),
         $(go.TextBlock, {
             editable: true, text: "label", font: "bold 14px iransans, sans-serif"
         }, // Link label
         new go.Binding("text", "text").makeTwoWay())
      );

    var dataAssociationLinkTemplate =
      $(go.Link,
        {
            routing: go.Link.AvoidsNodes, curve: go.Link.JumpGap, corner: 10,
            fromSpot: go.Spot.AllSides, toSpot: go.Spot.AllSides,
            reshapable: true, relinkableFrom: true, relinkableTo: true
        },
        new go.Binding("points").makeTwoWay(),
        $(go.Shape, { stroke: "black", strokeWidth: 1, strokeDashArray: [1, 3] }),
        $(go.Shape, { toArrow: "OpenTriangle", scale: 1, fill: null, stroke: "blue" })
     );

    var annotationAssociationLinkTemplate =
      $(go.Link,
        {
            reshapable: true, relinkableFrom: true, relinkableTo: true,
            toSpot: go.Spot.AllSides,
            toEndSegmentLength: 20, fromEndSegmentLength: 40
        },
        new go.Binding("points").makeTwoWay(),
        $(go.Shape, { stroke: "black", strokeWidth: 1, strokeDashArray: [1, 3] }),
        $(go.Shape, { toArrow: "OpenTriangle", scale: 1, stroke: "black" })
     );

    var linkTemplateMap = new go.Map("string", go.Link);
    linkTemplateMap.add("msg", messageFlowLinkTemplate);
    linkTemplateMap.add("annotation", annotationAssociationLinkTemplate);
    linkTemplateMap.add("data", dataAssociationLinkTemplate);
    linkTemplateMap.add("", sequenceLinkTemplate);  // default


    //------------------------------------------the main Diagram----------------------------------------------

    window.myDiagram =
      $(go.Diagram, "myDiagramDiv",
        {
            nodeTemplateMap: nodeTemplateMap,
            linkTemplateMap: linkTemplateMap,
            groupTemplateMap: groupTemplateMap,

            allowDrop: true,  // accept drops from palette

            commandHandler: new DrawCommandHandler(),  // defined in DrawCommandHandler.js
            // default to having arrow keys move selected nodes
            "commandHandler.arrowKeyBehavior": "move",

            mouseDrop: function (e) {
                // when the selection is dropped in the diagram's background,
                // make sure the selected Parts no longer belong to any Group
                var ok = myDiagram.commandHandler.addTopLevelParts(myDiagram.selection, true);
                if (!ok) myDiagram.currentTool.doCancel();
            },
            linkingTool: new BPMNLinkingTool(), // defined in BPMNClasses.js
            "SelectionMoved": function () { relayoutDiagram(myDiagram) },  // defined below
            "SelectionCopied": function () { relayoutDiagram(myDiagram) },

            //"animationManager.isEnabled": true,  // don't bother with layout animation
            contentAlignment: go.Spot.Center,  // content is always centered in the viewport
            autoScale: go.Diagram.Uniform  // scale always has all content fitting in the viewport
            //isReadOnly: true,  // don't let users modify anything
            // ,mouseOver: doMouseOver,  // this event handler is defined below
            // click: doMouseOver  // this event handler is defined below
        });

    myDiagram.toolManager.mouseDownTools.insertAt(0, new LaneResizingTool());

    myDiagram.addDiagramListener("LinkDrawn", function (e) {
        if (e.subject.fromNode.category === "annotation") {
            e.subject.category = "annotation"; // annotation association
        } else if (e.subject.fromNode.category === "dataobject" || e.subject.toNode.category === "dataobject") {
            e.subject.category = "data"; // data association
        } else if (e.subject.fromNode.category === "datastore" || e.subject.toNode.category === "datastore") {
            e.subject.category = "data"; // data association
        }
    });

    // change the title to indicate that the diagram has been modified
    myDiagram.addDiagramListener("Modified", function (e) {
        var currentFile = document.getElementById("currentFile");
        var idx = currentFile.textContent.indexOf("*");
        if (myDiagram.isModified) {
            if (idx < 0) currentFile.textContent = currentFile.textContent + "*";
        } else {
            if (idx >= 0) currentFile.textContent = currentFile.textContent.substr(0, idx);
        }
    });


    //------------------------------------------  Palette   ----------------------------------------------

    // Make sure the pipes are ordered by their key in the palette inventory
    function keyCompare(a, b) {
        var at = a.data.key;
        var bt = b.data.key;
        if (at < bt) return -1;
        if (at > bt) return 1;
        return 0;
    }

    // initialize the first Palette, BPMN Spec Level 1
    var myPaletteLevel = $(go.Palette, "myPaletteLevel",
        { // share the templates with the main Diagram
            nodeTemplateMap: palNodeTemplateMap,
            groupTemplateMap: palGroupTemplateMap,
            layout: $(go.GridLayout,
                      {
                          cellSize: new go.Size(1, 1),
                          spacing: new go.Size(5, 5),
                          comparer: keyCompare
                      })
        });

    jQuery("#accordion").accordion({
        activate: function (event, ui) {
            myPaletteLevel.requestUpdate();
        }
    });

    myPaletteLevel.model = $(go.GraphLinksModel,
      {
          copiesArrays: true,
          copiesArrayObjects: true,
          nodeDataArray: [
          // -------------------------- Event Nodes
            { category: "event", text: "Start", eventType: 1, eventDimension: 1 },
            { category: "event", text: "Message", eventType: 2, eventDimension: 2 }, // BpmnTaskMessage
            { category: "event", text: "Timer", eventType: 3, eventDimension: 3 },
            { category: "event", text: "End", eventType: 1, eventDimension: 8 },
            { category: "event", text: "Message", eventType: 2, eventDimension: 8 },// BpmnTaskMessage
            { category: "event", text: "Terminate", eventType: 13, eventDimension: 8 },
          // -------------------------- Task/Activity Nodes
            { key: 131, category: "activity", text: "Task", item: "generic task", taskType: 0 },
            { key: 132, category: "activity", text: "User Task", item: "User task", taskType: 2 },
            { key: 133, category: "activity", text: "Service\nTask", item: "service task", taskType: 6 },
          // subprocess and start and end
            { key: 134, category: "subprocess", loc: "0 0", text: "Subprocess", isGroup: true, isSubProcess: true, taskType: 0 },
              { key: -802, category: "event", loc: "0 0", group: 134, text: "Start", eventType: 1, eventDimension: 1, item: "start" },
              { key: -803, category: "event", loc: "350 0", group: 134, text: "End", eventType: 1, eventDimension: 8, item: "end", name: "end" },
          // -------------------------- Gateway Nodes, Data, Pool and Annotation
            { key: 201, category: "gateway", text: "Parallel", gatewayType: 1 },
            { key: 204, category: "gateway", text: "Exclusive", gatewayType: 4 },
            { key: 301, category: "dataobject", text: "Data\nObject" },
            { key: 302, category: "datastore", text: "Data\nStorage" },
            { key: 401, category: "privateProcess", text: "Black Box" },

            { key: "501", "text": "Pool 1", "isGroup": "true", "category": "Pool" },
              { key: "Lane5", "text": "Lane 1", "isGroup": "true", "group": "501", "color": "lightyellow", "category": "Lane" },
              { key: "Lane6", "text": "Lane 2", "isGroup": "true", "group": "501", "color": "lightgreen", "category": "Lane" },

            { key: 701, category: "annotation", text: "note" }
          ]  // end nodeDataArray
      });  // end model

    //------------------------------------------  Overview   ----------------------------------------------

    var myOverview = $(go.Overview, "myOverviewDiv",
        { observed: myDiagram, maxScale: 0.5, contentAlignment: go.Spot.Center });
    // change color of viewport border in Overview
    myOverview.box.elt(0).stroke = "dodgerblue";
    

    return myDiagram;
} // end init





/*

var lastStroked = null;  // this remembers the last highlit Shape

// Make sure the infoBox is momentarily hidden if the user tries to mouse over it
    var infoBoxH = document.getElementById("infoBoxHolder");
    infoBoxH.addEventListener("mousemove", function () {
        var box = document.getElementById("infoBoxHolder");
        box.style.left = parseInt(box.style.left) + "px";
        box.style.top = parseInt(box.style.top) + 30 + "px";
    }, false);

// Make sure the infoBox is hidden when the mouse is not over the Diagram
// var diagramDiv = document.getElementById("myDiagramDiv");
diagramDiv.addEventListener("mouseout", function (e) {
    if (lastStroked !== null) lastStroked.stroke = null;
    lastStroked = null;

    var infoBox = document.getElementById("infoBox");
    var elem = document.elementFromPoint(e.clientX, e.clientY);
    if (elem !== null && (elem === infoBox || elem.parentNode === infoBox)) {
        // do nothing
    } else {
        var box = document.getElementById("infoBoxHolder");
        box.innerHTML = "";
    }
}, false);


// Called when the mouse is over the diagram's background
function doMouseOver(e) {
    if (e === undefined) e = myDiagram.lastInput;
    var doc = e.documentPoint;
    // find all Nodes that are within 100 units
    var list = myDiagram.findObjectsNear(doc, 4, null, function (x) { return x instanceof go.Node; });
    // now find the one that is closest to e.documentPoint
    var closest = null;
    var closestDist = 999999999;
    list.each(function (node) {
        var dist = doc.distanceSquaredPoint(node.getDocumentPoint(go.Spot.Center));
        if (dist < closestDist) {
            closestDist = dist;
            closest = node;
        }
    });
    //highlightNode(e, closest);
    if (closest !== undefined && closest !== null)
        updateInfoBox(e.viewPoint, closest.data);
}

// This function is called to update the tooltip information
// depending on the bound data of the Node that is closest to the pointer.
function updateInfoBox(mousePt, data) {
    var x =
    "<div id='infoBox'>" +
        "<div class='tooltipTitle'>" + data.text + "</div>" +
        "<div class='aline'><div class='infoTitle'>Category</div><div class='infoValues'>" + data.category + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>Key</div><div class='infoValues'>" + data.key + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>eventDimension</div><div class='infoValues'>" + data.eventDimension + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>eventType</div><div class='infoValues'>" + data.eventType + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>gatewayType</div><div class='infoValues'>" + data.gatewayType + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>loc</div><div class='infoValues'>" + data.loc + "</div></div>" +
        "<div class='aline'><div class='infoTitle'>taskType</div><div class='infoValues'>" + data.taskType + "</div></div>";

    if (data.attributes !== undefined && data.attributes !== null) {
        x += "<div class='aline'><div class='infoTitle'>------ Detials ------</div><div class='infoValues'>----</div></div>";
        for (var prop in data.attributes) {
            x += "<div class='aline'><div class='infoTitle'>" + prop + "</div><div class='infoValues'>" + data.attributes[prop] + "</div></div>";
        }
    }

    x += "</div>";

    var box = document.getElementById("infoBoxHolder");
    box.innerHTML = x;

    box.style.left = mousePt.x + 160 + "px";
    box.style.top = mousePt.y + 30 + "px";
}
*/