--vip�ȼ��ɾ�
x100003_g_scriptId = 100003


--�¼�����
function x100003_OnEventFilter(uuid, achEventId, vipLevel)

	if vipLevel ~= nil and tonumber(vipLevel) <= LuaGetVipLevel(uuid) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100003_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end

