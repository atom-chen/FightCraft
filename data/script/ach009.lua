--�߼���ʯ
x100009_g_scriptId = 100009

--�¼�����
function x100009_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 7) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100009_OnAchievementProgress(uuid, reachNum)
  	return 1
end

