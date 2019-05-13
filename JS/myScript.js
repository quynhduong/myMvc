$.ajax({
    dataType: "json",
    contentType: "application/json",
    type: 'POST',
    url: '/Controller/Action',
    data: { 'items': JSON.stringify(lineItems), 'id': documentId }
});