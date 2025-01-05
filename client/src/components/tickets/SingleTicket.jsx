import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getTicketById } from "../../managers/ticketManager"; // Create a new API method to fetch single ticket

export default function SingleTicket() {
  const { ticketId } = useParams(); // Get ticketId from the URL
  const [ticket, setTicket] = useState(null);

  useEffect(() => {
    const fetchTicket = async () => {
      try {
        const data = await getTicketById(ticketId);
        setTicket(data);
      } catch (error) {
        console.error("Error fetching ticket:", error);
      }
    };

    fetchTicket();
  }, [ticketId]);

  if (!ticket) {
    return <p>Loading ticket details...</p>;
  }

  return (
    <div className="text-white">
      <h2>{ticket.subject}</h2>
      <p>
        <strong>Category:</strong> {ticket.category}
      </p>
      <p>
        <strong>Game:</strong> {ticket.game}
      </p>
      <p>
        <strong>Server:</strong> {ticket.server}
      </p>
      <p>
        <strong>Description:</strong> {ticket.description}
      </p>
      <p>
        <strong>Assigned Users:</strong>{" "}
        {ticket.assignedUsers.map((user) => (
          <span key={ticket.id}>
            {user.firstName} {user.lastName}
            {", "}
          </span>
        ))}
      </p>
    </div>
  );
}
