import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getOpenTickets, closeTicketAPI } from "../../managers/ticketManager";

export default function OpenTickets({ onTicketChange }) {
  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState(null);

  const navigate = useNavigate();

  const fetchTickets = async () => {
    try {
      const data = await getOpenTickets();
      setTickets(data);
    } catch (error) {
      console.error("Error fetching open tickets:", error);
      setError("Failed to fetch open tickets. Please try again.");
    }
  };

  useEffect(() => {
    fetchTickets();
  }, []);

  const handleTicketClick = (ticketId) => {
    navigate(`/tickets/ticket/${ticketId}`);
  };

  const handleCloseTicket = async (ticketId) => {
    try {
      await closeTicketAPI(ticketId);
      setTickets((prevTickets) =>
        prevTickets.filter((ticket) => ticket.id !== ticketId)
      );
      onTicketChange();
    } catch (error) {
      console.error("Error closing ticket:", error);
    }
  };

  function truncateText(text, maxLength = 20) {
    return text.length <= maxLength ? text : `${text.slice(0, maxLength)}...`;
  }

  if (error) {
    return <p className="text-danger">{error}</p>;
  }

  return (
    <div>
      {tickets.length === 0 ? (
        <p className="mt-5 pt-4 text-white">You have no open tickets.</p>
      ) : (
        <table className="table table-dark table-striped align-middle">
          <thead className="thead-dark">
            <tr>
              <th className="text-start col-4">Topic</th>
              <th className="text-start col-1">Game</th>
              <th className="text-start col-2">Server</th>
              <th className="text-start col-1">Status</th>
              <th className="text-end col-2 pe-3">Options</th>
            </tr>
          </thead>
          <tbody>
            {tickets
              .slice()
              .reverse()
              .map((ticket) => (
                <tr
                  key={ticket.id}
                  onClick={() => handleTicketClick(ticket.id)}
                  style={{ cursor: "pointer" }}
                >
                  <td className="text-start col-4">
                    <div>
                      <strong className="text-white">{ticket.subject}</strong>
                      <br />
                      <small className="sub-text">{ticket.category}</small>
                      <br />
                      <small className="sub-text">
                        {new Date(ticket.createdAt).toLocaleString()}
                      </small>
                      <br />
                      <small className="sub-text">
                        Assigned:{" "}
                        {ticket.assignedUsers
                          .map((user) => `${user.firstName} ${user.lastName}`)
                          .join(", ")}
                      </small>
                    </div>
                  </td>
                  <td className="text-start col-1">
                    <span className="text-warning fw-bold">{ticket.game}</span>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-warning fw-bold">
                      {truncateText(ticket.server)}
                    </span>
                  </td>
                  <td className="text-start col-1">
                    <span className="text-success fw-bold">
                      {ticket.status}
                    </span>
                  </td>
                  <td className="text-start col-1 position-relative">
                    <div className="d-flex justify-content-end pe-2">
                      <button
                        className="btn btn-danger btn-sm ticket-button"
                        onClick={(e) => {
                          e.stopPropagation();
                          handleCloseTicket(ticket.id);
                        }}
                      >
                        <i className="bi bi-x-circle"></i>
                      </button>
                    </div>
                    <small className="position-absolute ticket-id">
                      Ticket ID: {ticket.id}
                    </small>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
