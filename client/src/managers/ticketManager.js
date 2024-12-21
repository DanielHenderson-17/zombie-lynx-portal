const _apiUrl = "/api/tickets";

export const getOpenTickets = () => {
  return fetch(_apiUrl + "/open").then((res) => res.json());
};

export const createTicket = (ticket) => {
  return fetch(_apiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(ticket),
  }).then((res) => res.json());
};

export const updateTicket = (ticket) => {
  return fetch(`${_apiUrl}/${ticket.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(ticket),
  });
};

export const closeTicket = (ticketId) => {
  return fetch(`${_apiUrl}/${ticketId}/close`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  });
};

export const deleteTicket = (ticketId) => {
  return fetch(`${_apiUrl}/${ticketId}`, {
    method: "DELETE",
  });
};
